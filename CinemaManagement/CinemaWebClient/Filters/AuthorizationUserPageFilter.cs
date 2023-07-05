﻿using CinemaWebClient.Utils;
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

		public AuthorizationUserPageFilter()
		{
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

			allowAdmin.Add("/Admin/Index");
			allowAdmin.Add("/Admin/Genre/Index");
			allowAdmin.Add("/Admin/Show/Index");
			allowAdmin.Add("/Admin/Room/Index");
			allowAdmin.Add("/Admin/Booking/Index");
			allowAdmin.Add("/Admin/User/Index");
			allowAdmin.Add("/Admin/Film/Index");
			allowAdmin.Add("/Admin/HistoryTransaction/Index");
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
			await Util.RefreshToken(context.HttpContext);
			string? userInfo = context.HttpContext.Session.GetString("info");
			//if (string.IsNullOrEmpty(userInfo))
			//{
			//	foreach (string urlFilter in allowAnonymous)
			//	{
			//		if (urlFilter.Contains(url))
			//		{
			//			await next.Invoke();
			//			return;
			//		}
			//	}
			//	context.Result = new RedirectToPageResult(previousUrl, dictinaryQuery);
			//	return;
			//}


			if (string.IsNullOrEmpty(userInfo))
			{
				context.Result = new RedirectToPageResult(previousUrl, dictinaryQuery);
				return;
			}
			UserSignInResponseDTO? user = JsonSerializer.Deserialize<UserSignInResponseDTO>(userInfo);

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
				foreach (string urlFilter in allowUser)
				{
					if (urlFilter.Contains(url))
					{
						await next.Invoke();
						return;
					}
				}
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
		
		public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
		{
			return Task.CompletedTask;
		}
	}
}
