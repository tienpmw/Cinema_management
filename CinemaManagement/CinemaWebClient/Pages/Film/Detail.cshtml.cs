using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Pages.Film
{
	public class DetailModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string FilmApi = string.Empty;

		public DetailModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			FilmApi = "http://localhost:5001/api/Films";
		}
		[BindProperty(SupportsGet = true)]
		public FilmDTO Film { get; set; }
		public async Task<IActionResult> OnGet(long id)
		{
			HttpResponseMessage response = await _httpClient.GetAsync($"{FilmApi}/GetFilmById/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToPage("/Home");
			}
			string data = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			Film = JsonSerializer.Deserialize<FilmDTO>(data, options) ?? new FilmDTO();
			Film.Shows = Film.Shows.Where(x => DateTime.Compare(x.ShowDate, DateTime.Now) > 0).OrderBy(x => x.ShowDate).ToList();
			return Page();
		}
	}
}
