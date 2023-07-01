using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

		[BindProperty]
		public ShowCreateDTO Show { get; set; }
		[ViewData]
		public SelectList? Rooms { get; set; }
		[ViewData]
		public SelectList? Films { get; set; }
		public async Task<IActionResult> OnGet()
		{
			//HttpResponseMessage response = await _httpClient.GetAsync(RoomApi);
			//string dataStr = await response.Content.ReadAsStringAsync();
			return Page();
		}
		public async Task<IActionResult> OnPost()
		{
			if(!ModelState.IsValid)
			{
				return Page();
			}
			var content = new StringContent(JsonSerializer.Serialize(Show), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync(ShowApi, content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = "Create Show Failed!";
			}
			TempData["SuccessMsg"] = "Create Show Success!";
			return RedirectToPage("/Admin/Show/Index");
		}
	}
}
