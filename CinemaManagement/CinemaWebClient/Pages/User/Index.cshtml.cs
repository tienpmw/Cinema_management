using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static Google.Apis.Requests.BatchRequest;

namespace CinemaWebClient.Pages.User
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public UserDTO userDTO { get; set; }

        private readonly HttpClient client;
        public IndexModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<IActionResult> OnGet()
        {
            var user = JsonSerializer.Deserialize<UserSignInResponseDTO>(HttpContext.Session.GetString("info"));
            var request = await client.GetAsync("http://localhost:5001/api/Users/" + user.UserId);
            var dataStr = await request.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            
            userDTO = JsonSerializer.Deserialize<UserDTO>(dataStr, options);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {

            var content = new StringContent(JsonSerializer.Serialize<UserDTO>(userDTO), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"http://localhost:5001/api/Users/EditUsername", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMsg"] = "Edit User Failed.";
            }
            else
            {
                TempData["SuccessMsg"] = "Edit User Success.";
            }
            return Page();
        }
    }
}
