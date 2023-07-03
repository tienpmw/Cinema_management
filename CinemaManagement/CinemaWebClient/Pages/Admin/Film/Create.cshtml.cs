using BusinessObject;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static Google.Apis.Requests.BatchRequest;

namespace CinemaWebClient.Pages.Admin.Film
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public FilmDTO FilmDTO { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<Genre>Genres { get; set; }
        [BindProperty(SupportsGet = true)] 
        public List<CountryDTO> Countries { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }    

        [BindProperty]
        public string Message { get; set; }

        private readonly HttpClient client = null;


        public CreateModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Message = string.Empty;
        }
        public async Task<IActionResult> OnGet()
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                HttpResponseMessage responeGenre = await client.GetAsync("http://localhost:5001/api/Genres");
                var strDataGenre = await responeGenre.Content.ReadAsStringAsync();
                
                HttpResponseMessage responeCountry = await client.GetAsync("http://localhost:5001/api/Countries");
                var strDataCountry = await responeCountry.Content.ReadAsStringAsync();

                Genres = JsonSerializer.Deserialize<List<Genre>>(strDataGenre, options);
                Countries = JsonSerializer.Deserialize<List<CountryDTO>>(strDataCountry, options);
            }
            catch (Exception)
            {
                Message = "Cannot connect to server!";
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove("Message");
            ModelState.Remove("ImageFile");
            ModelState.Remove("FilmDTO.FilmDuration");
            if (!ModelState.IsValid) return Page();

            HttpResponseMessage responeSubmitData = null;

            if (ImageFile != null && ImageFile.Length > 0)
            {
                using (var httpClient = client)
                {
                    var formData = new MultipartFormDataContent();
                    formData.Add(new StreamContent(ImageFile.OpenReadStream()), "imageFile", ImageFile.FileName);
                    var jsonData = JsonSerializer.Serialize(FilmDTO);
                    formData.Add(new StringContent(jsonData, Encoding.UTF8, "application/json"), "filmDTO");

                    var apiUrl = "http://localhost:5001/api/Films";
                    var response = await httpClient.PostAsync(apiUrl, formData);
                    responeSubmitData = response;
                    Message = await response.Content.ReadAsStringAsync();
                }
            }


            if (responeSubmitData.StatusCode != System.Net.HttpStatusCode.Conflict && responeSubmitData.StatusCode != System.Net.HttpStatusCode.OK)
            {
                Message = "Some thing went wrong! Try again!";
                return Page();
            }
            TempData["SuccessMsg"] = Message;
            return RedirectToPage("/Admin/Film/Index");
        }
    }
}
