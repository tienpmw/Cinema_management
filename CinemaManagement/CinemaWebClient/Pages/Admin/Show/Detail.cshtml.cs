using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Pages.Admin.Show
{
    public class DetailModel : PageModel
    {
		private readonly HttpClient _httpClient;
		private readonly string ShowApi = string.Empty;

        public DetailModel()
        {
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			ShowApi = "http://localhost:5001/api/Shows";
		}

		[BindProperty]
		public ShowDTO? Show { get; set; }

		public async Task<IActionResult> OnGet(long id)
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			HttpResponseMessage response = await _httpClient.GetAsync($"{ShowApi}/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToPage("/Home");
			}
			string data = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			Show = JsonSerializer.Deserialize<ShowDTO>(data, options) ?? new ShowDTO();
			return Page();
		}
	}
}
