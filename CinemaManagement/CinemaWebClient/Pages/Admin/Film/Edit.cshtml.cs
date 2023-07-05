using BusinessObject;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using System.Net.Http;
using CinemaWebClient.Utils;

namespace CinemaWebClient.Pages.Admin.Film
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public FilmDTO FilmDTO { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<BusinessObject.Genre> Genres { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<CountryDTO> Countries { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public string Message { get; set; }

        private readonly HttpClient client = null;


        public EditModel()
        {
            client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			Message = string.Empty;
        }
		
		public async Task<IActionResult> OnGet(int id)
        {
			Util.SetAuthenticationToken(client, HttpContext);
			var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                HttpResponseMessage responeFilm = await client.GetAsync("http://localhost:5001/api/Films/GetFilmById/" + id);
                var strDataFilm = await responeFilm.Content.ReadAsStringAsync();

                HttpResponseMessage responeGenre = await client.GetAsync("http://localhost:5001/api/Genres");
                var strDataGenre = await responeGenre.Content.ReadAsStringAsync();

                HttpResponseMessage responeCountry = await client.GetAsync("http://localhost:5001/api/Countries");
                var strDataCountry = await responeCountry.Content.ReadAsStringAsync();
                FilmDTO = JsonSerializer.Deserialize<FilmDTO>(strDataFilm, options);
                Genres = JsonSerializer.Deserialize<List<BusinessObject.Genre>>(strDataGenre, options);
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
			Util.SetAuthenticationToken(client, HttpContext);
			ModelState.Remove("Message");
            ModelState.Remove("ImageFile");
            ModelState.Remove("FilmDTO.FilmDuration");
            if (!ModelState.IsValid) return Page();

            HttpResponseMessage responeSubmitData = null;

            using (var httpClient = client)
            {
                var formData = new MultipartFormDataContent();
                if (ImageFile != null && ImageFile.Length > 0)
                {
                    formData.Add(new StreamContent(ImageFile.OpenReadStream()), "imageFile", ImageFile.FileName);
                }
                var jsonData = JsonSerializer.Serialize(FilmDTO);
                formData.Add(new StringContent(jsonData, Encoding.UTF8, "application/json"), "filmDTO");

                var apiUrl = "http://localhost:5001/api/Films";
                var response = await httpClient.PutAsync(apiUrl, formData);
                responeSubmitData = response;
                Message = await response.Content.ReadAsStringAsync();
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
