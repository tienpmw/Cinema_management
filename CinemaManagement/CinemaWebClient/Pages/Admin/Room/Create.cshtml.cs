using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualBasic;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static Google.Apis.Requests.BatchRequest;

namespace CinemaWebClient.Pages.Admin.Room
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public RoomDTO RoomDTO { get; set; }


        [BindProperty]
        public string Message { get; set; }
        [BindProperty]
        public bool StatusRequest { get; set; }

        private readonly HttpClient client;

        public CreateModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Message = null;
            StatusRequest = false;
        }

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Message");
            if (!ModelState.IsValid) return Page();
            
            var jsonData = JsonSerializer.Serialize(RoomDTO);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage request;
            try
            {
                request = await client.PostAsync("http://localhost:5001/api/Rooms", content);
                Message = await request.Content.ReadAsStringAsync();
                if (request.StatusCode != System.Net.HttpStatusCode.Conflict && request.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    Message = "Some thing went wrong! Try again!";
                }
                if (request.IsSuccessStatusCode) StatusRequest = true;
            }
            catch (HttpRequestException)
            {
                Message = "Cannot connect to server!";
            }
            return Page();
        }
    }
}
