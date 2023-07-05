using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Pages.Admin.User
{
	public class IndexModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string RoleApi = string.Empty;
		public IndexModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			RoleApi = "http://localhost:5001/api/Roles";
		}
		[ViewData]
		public SelectList Roles { get; set; }
		public async Task OnGet()
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			HttpResponseMessage response = await _httpClient.GetAsync(RoleApi);
			var dataStr = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			List<RoleDTO> rolesDTO = JsonSerializer.Deserialize<List<RoleDTO>>(dataStr, options) ?? new List<RoleDTO>();
			rolesDTO.Add(new RoleDTO
			{
				RoleId = -1,
				RoleName = "Select options"
			});
			Roles = new SelectList(rolesDTO.OrderBy(x => x.RoleId), "RoleId", "RoleName");
		}
	}
}
