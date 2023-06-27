using DTOs;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Pages
{
	public class SignInModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string UserApi = "";

		public SignInModel(HttpClient httpClient)
		{
			_httpClient = httpClient;
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			UserApi = "http://localhost:5001/api/Users";
		}

		public async Task OnGetAsync()
		{

		}

		public async Task<IActionResult> OnGetSignInGoogle(string idToken)
		{
			GoogleJsonWebSignature.Payload payload = await GoogleJsonWebSignature.ValidateAsync(idToken).ConfigureAwait(false);

			var model = new UserSignUpRequestDTO
			{
				Email = payload.Email,
				FirstName = payload.FamilyName,
				LastName = payload.GivenName
			};
			string dataStr = JsonSerializer.Serialize<UserSignUpRequestDTO>(model);
			var content = new StringContent(dataStr, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{UserApi}/SignInGoogle", content);
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToPage("/SignIn");

			}
			string jsonData = await response.Content.ReadAsStringAsync();
			UserSignInResponseDTO? userDTO = JsonSerializer.Deserialize<UserSignInResponseDTO>(jsonData);	
			if(userDTO.RoleName == "Admin")
			{
				return RedirectToPage("/Admin");
			} else
			{
				return RedirectToPage("/Home");
			}

		}
	}
}
