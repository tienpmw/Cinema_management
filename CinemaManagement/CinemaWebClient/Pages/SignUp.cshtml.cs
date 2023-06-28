using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Pages
{
	public class SignUpModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string UserApi = "";

		public SignUpModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			UserApi = "http://localhost:5001/api/Users";
		}
		[BindProperty(SupportsGet = true)]
		public UserSignUpRequestDTO SignUp { get; set; }

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPost()
		{
			if (!ModelState.IsValid)
			{
				return Page();
			}
			SignUp.Email = SignUp.Email.ToLower().Trim();
			string dataStr = JsonSerializer.Serialize(SignUp);
			var content = new StringContent(dataStr, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{UserApi}/SignUp", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["SuccessMsg"] = await response.Content.ReadAsStringAsync();
				return Page();
			}
			TempData["SuccessMsg"] = "Please Go To Email To Confirm The Account.";
			return RedirectToPage("/SignIn");
		}

		public async Task<IActionResult> OnGetConfirmEmail(string confirmToken, string email)
		{
			var request = new ConfirmEmailRequestDTO
			{
				Email = email,
				ConfirmToken = confirmToken
			};
			string dataStr = JsonSerializer.Serialize(request);
			var content = new StringContent(dataStr, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{UserApi}/ConfirmEmail", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = "Confirm Email Failed, Try Later.";
			}
			TempData["SuccessMsg"] = "Confirm Email Success.";
			return RedirectToPage("/SignIn");
		}

	}
}
