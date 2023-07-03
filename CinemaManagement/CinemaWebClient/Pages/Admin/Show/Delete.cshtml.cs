using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

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
