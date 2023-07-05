using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Pages.Admin.Show
{
	public class IndexModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string RoomApi = string.Empty;
		public IndexModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			RoomApi = "http://localhost:5001/api/Rooms";
		}
		[ViewData]
		public List<RoomDTO> Rooms { get; set; }
		public async Task OnGet()
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			HttpResponseMessage response = await _httpClient.GetAsync(RoomApi);
			string dataStr = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			Rooms = JsonSerializer.Deserialize<List<RoomDTO>>(dataStr, options) ?? new List<RoomDTO>();
		}
	}
}
