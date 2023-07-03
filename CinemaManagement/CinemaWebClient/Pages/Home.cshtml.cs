using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;

namespace CinemaWebClient.Pages
{
    public class HomeModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string FilmApi = string.Empty;

		public HomeModel()
		{
			_httpClient = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Accept.Add(contentType);
            FilmApi = "http://localhost:5001/api/Films";
		}

		public async Task OnGet()
        {

        }
    }
}
