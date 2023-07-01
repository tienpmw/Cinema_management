using DTOs;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Pages
{
	public class SignInModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string UserApi = "";

		public SignInModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			UserApi = "http://localhost:5001/api/Users";
		}
		[BindProperty(SupportsGet = true)]
		public UserSignInRequestDTO UserSignIn { get; set; }
		public async Task OnGetAsync()
		{

		}
		public async Task<IActionResult> OnPostAsync()
		{
			string dataStr = JsonSerializer.Serialize<UserSignInRequestDTO>(UserSignIn);
			var content = new StringContent(dataStr, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{UserApi}/SignIn", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = await response.Content.ReadAsStringAsync();
				return RedirectToPage("/SignIn");

			}
			string jsonData = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			UserSignInResponseDTO? userDTO = JsonSerializer.Deserialize<UserSignInResponseDTO>(jsonData, options);
			HttpContext.Session.SetString("info", JsonSerializer.Serialize(userDTO));
			if (userDTO.RoleName == "Admin")
			{
				return RedirectToPage("/Admin/Index");
			}
			else
			{
				return RedirectToPage("/Home");
			}
		}
		public async Task<IActionResult> OnGetSignInGoogle(string idToken)
		{
			GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken).ConfigureAwait(false);

			var model = new UserSignUpRequestDTO
			{
				Email = payload.Email,
				FirstName = payload.GivenName,
				LastName = payload.FamilyName
			};
			string dataStr = JsonSerializer.Serialize<UserSignUpRequestDTO>(model);
			var content = new StringContent(dataStr, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{UserApi}/SignInGoogle", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = await response.Content.ReadAsStringAsync();
				return RedirectToPage("/SignIn");

			}
			string jsonData = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			UserSignInResponseDTO? userDTO = JsonSerializer.Deserialize<UserSignInResponseDTO>(jsonData, options);
			HttpContext.Session.SetString("info", JsonSerializer.Serialize(userDTO));
			if (userDTO.RoleName == "Admin")
			{
				return RedirectToPage("/Admin/Index");
			}
			else
			{
				return RedirectToPage("/Home");
			}
		}
	}
}
