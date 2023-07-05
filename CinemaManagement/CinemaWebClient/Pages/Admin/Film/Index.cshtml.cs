using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CinemaWebClient.Pages.Admin.Film
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string GenreApi = string.Empty;
        public IndexModel()
        {
            _httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			GenreApi = "http://localhost:5001/api/Genres";
        }
        public SelectList Genres { get; set; }
        public async Task OnGet()
        {
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			HttpResponseMessage response = await _httpClient.GetAsync(GenreApi);
            string data = await response.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            List<GenreDTO> genreDTOs = JsonSerializer.Deserialize<List<GenreDTO>>(data, option) ?? new List<GenreDTO>();
            genreDTOs.Add(new GenreDTO
            {
                GenreId = -1,
                GenreName = "Select option"
            });

            Genres = new SelectList(genreDTOs.OrderBy(x => x.GenreId), "GenreId", "GenreName");
        }
    }
}
