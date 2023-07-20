using CinemaWebClient.Utils;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Pages.Film.Booking
{
	public class IndexModel : PageModel
	{
		private readonly HttpClient _httpClient;
		private readonly string ShowApi = string.Empty;
		private readonly string BookingApi = string.Empty;

		public IndexModel()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			ShowApi = "http://localhost:5001/api/Shows";
			BookingApi = "http://localhost:5001/api/Bookings";
		}
		[BindProperty]
		public ShowDTO? Show { get; set; }

		public async Task<IActionResult> OnGet(long id)
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			HttpResponseMessage response = await _httpClient.GetAsync($"{ShowApi}/{id}");
			if (!response.IsSuccessStatusCode)
			{
				return RedirectToPage("/Home");
			}
			string data = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
			};
			Show = JsonSerializer.Deserialize<ShowDTO>(data, options) ?? new ShowDTO();
			return Page();
		}
		public async Task<IActionResult> OnPost(long id)
		{
			Util.SetAuthenticationToken(_httpClient, HttpContext);
			string?[] seats = Request.Form["seats"].ToArray();
			if (seats == null || seats.Length == 0)
			{
				HttpResponseMessage response1 = await _httpClient.GetAsync($"{ShowApi}/{id}");
				if (!response1.IsSuccessStatusCode)
				{
					return RedirectToPage("/Home");
				}
				string data = await response1.Content.ReadAsStringAsync();
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				Show = JsonSerializer.Deserialize<ShowDTO>(data, options) ?? new ShowDTO();
				ModelState.RemoveAll<ShowDTO>(x => x);
				ModelState.AddModelError("", "Please select seat!");
				return Page();
			}
			var userInfo = JsonSerializer.Deserialize<UserSignInResponseDTO>(HttpContext.Session.GetString("info"));
			BookingRequestDTO booking = new BookingRequestDTO
			{
				UserId = userInfo.UserId,
				SeatsBooking = seats
			};
			var content = new StringContent(JsonSerializer.Serialize(booking), Encoding.UTF8, "application/json");
			HttpResponseMessage response = await _httpClient.PostAsync($"{BookingApi}/{id}", content);
			if (!response.IsSuccessStatusCode)
			{
				TempData["ErrorMsg"] = await response.Content.ReadAsStringAsync();
			}
			else
			{
				TempData["SuccessMsg"] = "Booking seats success!";
			}
			return RedirectToPage("/Film/Booking/Index", new { id = id });
		}
	}
}
