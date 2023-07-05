using DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CinemaWebClient.Filters
{
	public class AuthorizationUserPageFilter : IAsyncPageFilter
	{

		private List<string> allowAnonymous = new List<string>();
		private List<string> allowUser = new List<string>();
		private List<string> allowAdmin = new List<string>();

		private HttpClient _httpClient;
		private readonly string UserApi = string.Empty;

		public AuthorizationUserPageFilter()
		{
			_httpClient = new HttpClient();
			var contentType = new MediaTypeWithQualityHeaderValue("application/json");
			_httpClient.DefaultRequestHeaders.Accept.Add(contentType);
			UserApi = "http://localhost:5001/api/Users/RefreshToken";
			AddRequestAccept();
		}

		private void AddRequestAccept()
		{
			allowAnonymous.Add("/SignIn");
			allowAnonymous.Add("/SignUp");
			allowAnonymous.Add("/AccessDenied");
			allowAnonymous.Add("/Home");
			allowAnonymous.Add("/Film/Detail");

			allowUser.Add("/Film/Booking/Index");
            allowUser.Add("/Transaction/Index");
            allowUser.Add("/Transaction/Payment");
            allowUser.Add("/User/Index");
            allowUser.Add("/User/HistoryBuyTickets");
            allowUser.Add("/User/HistoryRecharges");


            allowAdmin.Add("/Admin/Index");
			allowAdmin.Add("/Admin/Genre/Index");
			allowAdmin.Add("/Admin/Show/Index");
			allowAdmin.Add("/Admin/Room/Index");
			allowAdmin.Add("/Admin/Booking/Index");
			allowAdmin.Add("/Admin/User/Index");
			allowAdmin.Add("/Admin/Film/Index");
			allowAdmin.Add("/Admin/Transaction/Index");
		}


		public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
		{
			//not check request
			//await next.Invoke();
			//return;


			string? url = context.HttpContext.Request.RouteValues["page"].ToString();
			string previousUrl = context.HttpContext.Request.Headers["Referer"].ToString();
			if (previousUrl == "http://localhost:5006/")
			{
				previousUrl = "/Home";
			}
			else
			{
				previousUrl = previousUrl.Replace("http://localhost:5006", "");
			}
			var dictinaryQuery = new Dictionary<string, string>();
			if (previousUrl.Contains("?"))
			{
				string[] queries = previousUrl.Replace(previousUrl.Substring(0, previousUrl.IndexOf("?") + 1), "").Split("&");
				foreach (string query in queries)
				{
					string[] pairKeyValue = query.Split("=");
					dictinaryQuery.Add(pairKeyValue[0], pairKeyValue[1]);
				}
				previousUrl = previousUrl.Substring(0, previousUrl.IndexOf("?"));
			}
			// check request for Anonymous
			foreach (string urlFilter in allowAnonymous)
			{
				if (urlFilter == url)
				{
					await next.Invoke();
					return;
				}
			}

			// check user
			string? userInfo = context.HttpContext.Session.GetString("info");
			if (userInfo == null)
			{
				context.Result = new RedirectToPageResult("/AccessDenied");
				return;
			}

            UserSignInResponseDTO? user = JsonSerializer.Deserialize<UserSignInResponseDTO>(userInfo);
			// check token expire then refresh token
			if (!string.IsNullOrEmpty(userInfo))
			{
				await RefreshToken(context);
			}

			// check request for user's role user
			if (user.RoleName.ToLower() == "user")
			{

				foreach (string urlFilter in allowUser)
				{
					if (urlFilter.Contains(url))
					{
						await next.Invoke();
						return;
					}
				}
				context.Result = new RedirectToPageResult("/AccessDenied");
				return;
			}

			//otherwise for admin
			// check request for user's role admin
			if (user.RoleName.ToLower() == "admin")
			{
				await RefreshToken(context);
				foreach (string urlFilter in allowAdmin)
				{
					if (urlFilter.Contains(url))
					{
						await next.Invoke();
						return;
					}
				}
				context.Result = new RedirectToPageResult("/AccessDenied");
				return;
			}

			await next.Invoke();
		}
		private async Task RefreshToken(PageHandlerExecutingContext context)
		{
			if (context.HttpContext.Session.GetString("info") != null)
			{
				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				UserSignInResponseDTO userInfo = JsonSerializer.Deserialize<UserSignInResponseDTO>(context.HttpContext.Session.GetString("info"), options) ?? new UserSignInResponseDTO();
				_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(userInfo.AccessToken ?? "");
				RefreshTokenRequestDTO rf = new RefreshTokenRequestDTO
				{
					AccessToken = userInfo.AccessToken,
					RefreshToken = userInfo.RefreshToken
				};
				var content = new StringContent(JsonSerializer.Serialize(rf), Encoding.UTF8, "application/json");
				HttpResponseMessage response = await _httpClient.PostAsync(UserApi, content);
				if (response.IsSuccessStatusCode)
				{
					context.HttpContext.Session.SetString("info", await response.Content.ReadAsStringAsync());
				}
			}
		}
		public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
		{
			return Task.CompletedTask;
		}
	}
}
