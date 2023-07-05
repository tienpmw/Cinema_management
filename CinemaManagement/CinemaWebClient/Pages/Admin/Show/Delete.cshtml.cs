using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Pages.Admin.Show
{
	public class DeleteModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string ShowApi = string.Empty;
		public DeleteModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			ShowApi = "http://localhost:5001/api/Shows";
		}
		public async Task<IActionResult> OnGet(long id)
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			HttpResponseMessage response = await _httpClient.DeleteAsync($"{ShowApi}/{id}");
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = "Delete Show Failed!";
			}
			else
			{
				TempData["SuccessMsg"] = "Delete Show Success!";
			}
			return RedirectToPage("/Admin/Show/Index");
		}
	}
}
