using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Pages.Admin.Show
{
	public class CreateModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string ShowApi = string.Empty;
		private readonly string RoomApi = string.Empty;
		private readonly string FilmApi = string.Empty;
		public CreateModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			ShowApi = "http://localhost:5001/api/Shows";
			RoomApi = "http://localhost:5001/api/Rooms";
			FilmApi = "http://localhost:5001/api/Films";
		}

		[BindProperty(SupportsGet = true)]
		public ShowCreateDTO Show { get; set; }
		[BindProperty(SupportsGet = true)]
		public List<RoomDTO>? Rooms { get; set; }
		[BindProperty(SupportsGet = true)]
		public List<FilmDTO>? Films { get; set; }
		public async Task<IActionResult> OnGet()
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			Show.ShowDate = DateTime.Now.Date;
			await LoadData();
			return Page();
		}
		public async Task<IActionResult> OnPost()
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			if (!ModelState.IsValid)
			{
				await LoadData();
				return Page();
			}
			var content = new StringContent(JsonSerializer.Serialize(Show), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync(ShowApi, content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = "Create Show Failed!";
			}
			else
			{
				TempData["SuccessMsg"] = "Create Show Success!";
			}
			return RedirectToPage("/Admin/Show/Index");
		}

		private async Task LoadData()
		{
			HttpResponseMessage response = await _httpClient.GetAsync(RoomApi);
			string dataStr = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			Rooms = JsonSerializer.Deserialize<List<RoomDTO>>(dataStr, options);
			response = await _httpClient.GetAsync(FilmApi);
			dataStr = await response.Content.ReadAsStringAsync();
			Films = JsonSerializer.Deserialize<List<FilmDTO>>(dataStr, options);

		}
	}
}
