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

namespace CinemaWebClient.Pages.Admin.Film
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public FilmDTO FilmDTO { get; set; }
        [BindProperty]
        public List<Genre>Genres { get; set; }
        [BindProperty] 
        public List<CountryDTO> Countries { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }    

        [BindProperty]
        public bool StatusRequest { get; set; }
        [BindProperty]
        public string Message { get; set; }

        private readonly HttpClient client = null;


        public CreateModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Message = string.Empty;
            StatusRequest = false;
            Genres = new List<Genre>(); 
            Countries = new List<CountryDTO>();    
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

        public async Task<IActionResult> OnPost()
        {
            ModelState.Remove("Message");
            ModelState.Remove("ImageFile");
            if(!ModelState.IsValid) return Page();


            using (var multipartFormContent = new MultipartFormDataContent())
            {
                multipartFormContent.Add(new StreamContent(ImageFile.OpenReadStream()), "imageFile");
                var jsonData = JsonSerializer.Serialize(FilmDTO);
                multipartFormContent.Add(new StringContent(jsonData, Encoding.UTF8, "application/json"), "filmDTO");
                var response = await client.PostAsync("http://localhost:5001/api/Films", multipartFormContent);
            }
            


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
    }
}
