using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
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
		private readonly ISendMailRepository _sendMailRepository;
		private readonly IUserRepository _userRepository;
		private readonly IRefreshTokenRepository _refreshTokenRepository;

		public UsersController(IConfiguration configuration, ISendMailRepository sendMailRepository,
			IUserRepository userRepository, IRefreshTokenRepository refreshTokenRepository)
		{
			_configuration = configuration;
			_sendMailRepository = sendMailRepository;
			_userRepository = userRepository;
			_refreshTokenRepository = refreshTokenRepository;
		}

		[EnableQuery]
		[ODataAttributeRouting]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok();
		}

		[HttpPost("SignIn")]
		public async Task<IActionResult> SignIn(UserSignInRequestDTO userSignIn)
		{
			User? user = _userRepository.GetUserByEmail(userSignIn.Email);
			if (user == null)
			{
				return NotFound("Email or Password not correct!");
			}
			if (!user.IsActive)
			{
				return Conflict("This user was banned, contact admin!");
			}
			if (!user.IsConfirmEmail)
			{
				return Conflict("Let's confirm email before login!");
			}
			if (user.IsLoginGoogle)
			{
				return Conflict("Let's login account with google option!");
			}
			var response = await GenerateToken(user);
			return Ok(response);
		}
		[HttpPost("SignInGoogle")]
		public async Task<IActionResult> SignInGoogle(UserSignUpRequestDTO userSignIn)
		{
			User? user = _userRepository.GetUserByEmail(userSignIn.Email);
			if (user == null)
			{
				try
				{
					var newUser = new User
					{
						Email = userSignIn.Email,
						Password = null,
						FirstName = userSignIn.FirstName,
						LastName = userSignIn.LastName,
						IsConfirmEmail = true,
						IsLoginGoogle = true,
						AccountBalance = 0,
						IsActive = true,
						RoleId = 2
					};
					_userRepository.AddUser(newUser);
					user = _userRepository.GetUserByEmail(userSignIn.Email);
				}
				catch (Exception)
				{
					return Conflict("Login failed, try later!");
				}
			}
			if (!user.IsActive)
			{
				return Conflict("This user was banned, contact admin!");
			}
			if (!user.IsLoginGoogle)
			{
				return Conflict("Let's login account with password!");
			}
			var response = await GenerateToken(user);
			return Ok(response);
		}
		[HttpPost("SignUp")]
		public async Task<IActionResult> SignUp(UserSignUpRequestDTO userSignUp)
		{
			User? user = _userRepository.GetUserByEmail(userSignUp.Email);
			if (user != null)
			{
				return Conflict("Account has existed!");
			}
			try
			{
				var newUser = new User
				{
					Email = userSignUp.Email,
					Password = userSignUp.Password,
					FirstName = userSignUp.FirstName,
					LastName = userSignUp.LastName,
					IsConfirmEmail = false,
					IsLoginGoogle = false,
					AccountBalance = 0,
					IsActive = true,
					RoleId = 2
				};
				_userRepository.AddUser(newUser);
				user = _userRepository.GetUserByEmail(newUser.Email);
			}
			catch (Exception)
			{

				return Conflict("Something wrong, try later!");
			}
			string confirmToken = GenerateConfirmToken(user);
			string body = $"<div>Please click <a href='http://localhost:5006/SignUp?confirmToken={confirmToken}&handler=ConfirmEmail'>here</a> to confirm email!</div>";
			await _sendMailRepository.SendEmailAsync(user.Email, "Confirm SignUp Account", body);
			return Ok();
		}

		[HttpPost("ConfirmEmail")]
		public IActionResult ConfirmEmail([FromBody]string confirmToken)
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
			} else
			{
				return Conflict();
			}
			long utcexpireDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
			DateTime expireDate = ConvertUnitTimeToDateTime(utcexpireDate);
			if (expireDate < DateTime.UtcNow)
			{
				return Conflict("Token expired!");
			}

			string? email = tokenVerification.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email).Value;
			_userRepository.UpdateConfirmEmail(email);
			return Ok();
		}

		[HttpPost("RefreshToken")]
		public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO rfToken)
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
				ClaimsPrincipal tokenVerification = jwtTokenHandler.ValidateToken(rfToken.AccessToken,
					tokenValidateParam, out var validatedToken);

				// check algorithm
				if (validatedToken is JwtSecurityToken jwtSecurityToken)
				{
					bool result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512,
						StringComparison.InvariantCultureIgnoreCase);
					if (!result)
					{
						return Conflict("Access token not correct!");
					}
				} else
				{
					return Conflict("");
				}
				// check accessToken expire not yet
				long utcexpireDate = long.Parse(tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
				DateTime expireDate = ConvertUnitTimeToDateTime(utcexpireDate);
				if (expireDate > DateTime.UtcNow)
				{
					return Conflict("Access token not expired!");
				}

				// check refreshToken exist DB
				var refreshToken = _refreshTokenRepository.GetRefreshToken(rfToken.RefreshToken);
				if (refreshToken == null)
				{
					return Conflict("Refresh token not correct!");
				}

				// check refreshToken used yet
				if (refreshToken.IsUsed)
				{
					return Conflict("Refresh Token was used!");
				}

				// check accessToken id equal jwtId of refreshToken yet
				var jti = tokenVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
				if (refreshToken.JwtId != jti)
				{
					return Conflict("Id Token not match!");
				}
				try
				{
					_refreshTokenRepository.UpdateRefreshToken(refreshToken);
				}
				catch (Exception)
				{

					return Conflict("Something wrong, try later!");
				}

				//create new token
				var user = _userRepository.GetUserById(refreshToken.UserId);
				var token = await GenerateToken(user);
				return Ok(token);
			}
			catch (Exception ex)
			{
				return BadRequest();
			}
		}

		private string GenerateConfirmToken(User model)
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
				Expires = DateTime.UtcNow.AddMinutes(30),
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
			_refreshTokenRepository.AddRefreshToken(refreshTokenModel);
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
			dateTimeInterval = dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
			return dateTimeInterval;
		}
	}
}
