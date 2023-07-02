using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CinemaWebClient.Pages.Admin
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

		public async Task OnGet()
		{
			HttpResponseMessage response = await _httpClient.GetAsync(ShowApi + "/GetEarningMonthly");
			string dataStr = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true,
			};
			JsonObject data = JsonSerializer.Deserialize<JsonObject>(dataStr, options) ?? new JsonObject();
			ViewData["EarningMonthly"] = ((long)data["earning"]).ToString("C0").Substring(1);
			response = await _httpClient.GetAsync(ShowApi + "/GetEarningAnnual");
			dataStr = await response.Content.ReadAsStringAsync();
			JsonObject data2 = JsonSerializer.Deserialize<JsonObject>(dataStr, options) ?? new JsonObject();

			ViewData["EarningAnnual"] = (((JsonArray)data2["earning"])?.Sum(x => (long)x["earning"]) ?? 0).ToString("C0").Substring(1);
		}
	}
}
