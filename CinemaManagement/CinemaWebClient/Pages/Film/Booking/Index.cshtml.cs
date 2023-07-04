using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Pages.Film.Booking
{
	public class IndexModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string ShowApi = string.Empty;

		public IndexModel()
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
		public async Task<IActionResult> OnPost(long id)
		{
			string?[] seats = Request.Form["seats"].ToArray();
			if(seats == null || seats.Length == 0)
			{
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
				ModelState.RemoveAll<ShowDTO>(x => x);
				ModelState.AddModelError("", "Please select seat!");
				return Page();
			}

			return RedirectToPage("/Film/Booking/Index");
		}
	}
}
