using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CinemaWebClient.Pages.Admin.User
{
	public class EditModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string UserApi = string.Empty;
		private readonly string RoleApi = string.Empty;
		public EditModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			UserApi = "http://localhost:5001/api/Users";
			RoleApi = "http://localhost:5001/api/Roles";
		}

		[BindProperty]
		public UserDTO User { get; set; }

		[ViewData]
		public SelectList Roles { get; set; }
		[ViewData]
		public Dictionary<int, string> Status { get; set; }
		public async Task<IActionResult> OnGet(int id)
		{
			var response = await _httpClient.GetAsync($"{UserApi}/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToPage("/Admin/User/Index");
			}
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			string dataStr = await response.Content.ReadAsStringAsync();
			User = JsonSerializer.Deserialize<UserDTO>(dataStr, options) ?? new UserDTO();
			response = await _httpClient.GetAsync(RoleApi);
			dataStr = await response.Content.ReadAsStringAsync();
			List<RoleDTO> lsRole = JsonSerializer.Deserialize<List<RoleDTO>>(dataStr, options) ?? new List<RoleDTO>();
			Roles = new SelectList(lsRole, "RoleId", "RoleName", lsRole.FirstOrDefault(x => x.RoleId == User.RoleId));
			Status = new Dictionary<int, string>();
			Status.Add(0, "Ban");
			Status.Add(1, "Active");

			return Page();
		}
		public async Task<IActionResult> OnPost()
		{

			var content = new StringContent(JsonSerializer.Serialize<UserDTO>(User), Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync($"{UserApi}/Edit", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = "Edit User Failed.";
			}
			TempData["SuccessMsg"] = "Edit User Success.";
			return RedirectToPage("/Admin/User/Index");
		}
	}
}
