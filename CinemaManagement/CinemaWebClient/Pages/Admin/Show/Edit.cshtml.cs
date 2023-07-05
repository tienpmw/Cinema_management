using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Pages.Admin.Show
{
	public class EditModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string ShowApi = string.Empty;
		private readonly string RoomApi = string.Empty;
		private readonly string FilmApi = string.Empty;
		public EditModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			ShowApi = "http://localhost:5001/api/Shows";
			RoomApi = "http://localhost:5001/api/Rooms";
			FilmApi = "http://localhost:5001/api/Films";
		}

		[BindProperty(SupportsGet = true)]
		public ShowDTO Show { get; set; }
		public async Task<IActionResult> OnGet(long id)
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			HttpResponseMessage response = await _httpClient.GetAsync(ShowApi + "/" + id);
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToPage("/Admin/Show/Index");
			}
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			Show = JsonSerializer.Deserialize<ShowDTO>(await response.Content.ReadAsStringAsync(), options) ?? new ShowDTO();
			return Page();
		}
		public async Task<IActionResult> OnPost(long id)
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			if (ModelState["Show.ShowId"].Errors.Count != 0 || ModelState["Show.ShowDate"].Errors.Count != 0)
			{
				return Page();
			}
			if (id != Show.ShowId)
			{
				TempData["ErrorMsg"] = "Edit Show Failed!";
				return RedirectToPage("/Admin/Show/Index");
			}
			string data = JsonSerializer.Serialize(Show);
			var content = new StringContent(data, Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PutAsync(ShowApi, content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = "Edit Show Failed!";
			}
			else
			{
				TempData["SuccessMsg"] = "Edit Show Success!";
			}
			return RedirectToPage("/Admin/Show/Index");
		}
	}
}
