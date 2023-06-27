using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CinemaWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IConfiguration _configuration;
		private readonly CinemaContext _context;
		private readonly ISendMailRepository _sendMailRepository;
		private readonly IUserRepository _userRepository;

		public UsersController(IConfiguration configuration, CinemaContext context, ISendMailRepository sendMailRepository, IUserRepository userRepository)
		{
			_configuration = configuration;
			_context = context;
			_sendMailRepository = sendMailRepository;
			_userRepository = userRepository;
		}

		[EnableQuery]
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok();
		}

		[HttpPost("SignIn")]
		public async Task<IActionResult> SignIn(UserSignInRequestDTO userSignIn)
		{
			User? user = _context.User.FirstOrDefault(x => x.Email == userSignIn.Email);
			if (user == null)
			{
				return NotFound("Email or Password not correct!");
			}
			if (!user.IsActive)
			{
				return Conflict("This user was banned, contact admin!");
			}
			var response = await GenerateToken(user);
			return Ok(response);
		}
		[HttpPost("SignInGoogle")]
		public async Task<IActionResult> SignInGoogle(UserSignUpRequestDTO userSignIn)
		{
			User? user = _context.User.FirstOrDefault(x => x.Email == userSignIn.Email);
			if (user == null)
			{
				try
				{
					// Add user
					_userRepository.AddUserLoginGoogle(new User { Email = userSignIn.Email, FirstName = userSignIn.FirstName, LastName = userSignIn.LastName});
					// Add success
					user = _context.User.FirstOrDefault(x => x.Email == userSignIn.Email);
				}
				catch (Exception)
				{
					return Conflict("Login failed, try later!");
				}
			}
			var response = await GenerateToken(user);
			return Ok(response);
		}
		[HttpPost("SignUp")]
		public async Task<IActionResult> SignUp(User userSignUp)
		{
			string confirmToken = await GenerateConfirmToken(userSignUp);
			return Ok();
		}

		[HttpPost("ConfirmEmail")]
		public async Task<IActionResult> ConfirmEmail(string confirmToken)
		{
			// validate token
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
			var tokenValidateParam = new TokenValidationParameters
			{
				// tự cấp token
				ValidateIssuer = false,
				ValidateAudience = false,

				// ký vào token
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

				ClockSkew = TimeSpan.Zero,
				ValidateLifetime = false //ko kiểm tra token hết hạn
			};
			// check token valid format
			ClaimsPrincipal tokenVerification = jwtTokenHandler.ValidateToken(confirmToken,
				tokenValidateParam, out var validatedToken);
			// check algorithm
			if (validatedToken is JwtSecurityToken jwtSecurityToken)
			{
				bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
					StringComparison.InvariantCultureIgnoreCase);
				if (!result)
				{
					return Conflict();
				}
			}

			string email = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email).Value;

			return Ok();
		}

		[HttpPost("RefreshToken")]
		public async Task<IActionResult> RefreshToken(User uerSignIn)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
			var tokenValidateParam = new TokenValidationParameters
			{
				// tự cấp token
				ValidateIssuer = false,
				ValidateAudience = false,

				// ký vào token
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

				ClockSkew = TimeSpan.Zero,
				ValidateLifetime = false //ko kiểm tra token hết hạn
			};
			try
			{
				// check token valid format
				ClaimsPrincipal tokenVerification = jwtTokenHandler.ValidateToken("accesstoken",
					tokenValidateParam, out var validatedToken);

				// check algorithm
				if (validatedToken is JwtSecurityToken jwtSecurityToken)
				{
					bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
						StringComparison.InvariantCultureIgnoreCase);
					if (!result)
					{
						return Conflict();
					}
				}

				// check accessToken expire not yet
				long utcexpireDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
				DateTime expireDate = ConvertUnitTimeToDateTime(utcexpireDate);
				if (expireDate > DateTime.UtcNow)
				{
					return Conflict();
				}

				// check refreshToken exist DB
				var refreshToken = _context.RefreshToken.FirstOrDefault(x => x.TokenRefresh == "refreshToken");
				if (refreshToken == null)
				{
					return Conflict();
				}

				// check refreshToken used yet
				if (refreshToken.IsUsed)
				{
					return Conflict();
				}

				// check accessToken id equal jwtId of refreshToken yet
				var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
				if (refreshToken.JwtId != jti)
				{
					return Conflict();
				}

				refreshToken.IsUsed = true;
				_context.RefreshToken.Update(refreshToken);
				await _context.SaveChangesAsync();

				//create new token
				var user = await _context.User.FirstOrDefaultAsync(x => x.UserId == refreshToken.UserId);
				var token = await GenerateToken(user);

				return Ok(token);

			}
			catch (Exception ex)
			{
				return BadRequest();
			}
		}

		private async Task<string> GenerateConfirmToken(User model)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
			var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature);
			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Email, model.Email),
			};
			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(10),
				SigningCredentials = signingCredentials
			};
			var token = jwtTokenHandler.CreateToken(tokenDescription);
			return jwtTokenHandler.WriteToken(token);

		}
		private async Task<UserSignInResponseDTO> GenerateToken(User model)
		{
			var jwtTokenHandler = new JwtSecurityTokenHandler();
			var secretKeyBytes = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]);
			var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature);
			var claims = new[]
			{
				new Claim(ClaimTypes.Name, $"{model.FirstName} {model.LastName}"),
				new Claim(JwtRegisteredClaimNames.Email, model.Email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim("Id", model.UserId.ToString()),
				new Claim(ClaimTypes.Role, model.Role.RoleName)
			};
			var tokenDescription = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.UtcNow.AddMinutes(30),
				SigningCredentials = signingCredentials
			};
			var token = jwtTokenHandler.CreateToken(tokenDescription);
			var accessToken = jwtTokenHandler.WriteToken(token);
			var refreshToken = GenerateRefreshToken();

			var refreshTokenModel = new RefreshToken
			{
				UserId = model.UserId,
				TokenRefresh = refreshToken,
				JwtId = token.Id, // same jti
				IsUsed = false,
			};
			_context.RefreshToken.Add(refreshTokenModel);
			await _context.SaveChangesAsync();

			var response = new UserSignInResponseDTO
			{
				UserId = model.UserId,
				Email = model.Email,
				FirstName = model.FirstName,
				LastName = model.LastName,
				RoleName = model.Role.RoleName,
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
			return response;
		}

		private string GenerateRefreshToken()
		{
			var random = new byte[32];
			using (var rng = RandomNumberGenerator.Create())
			{
				rng.GetBytes(random);

				return Convert.ToBase64String(random);
			}
		}
		private DateTime ConvertUnitTimeToDateTime(long utcExpireDate)
		{
			var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();

			return dateTimeInterval;
		}
	}
}
