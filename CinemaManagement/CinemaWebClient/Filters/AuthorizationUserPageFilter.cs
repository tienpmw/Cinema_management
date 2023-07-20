using CinemaWebClient.Utils;
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
            allowAnonymous.Add("/SignOut");
            allowAnonymous.Add("/AccessDenied");
			allowAnonymous.Add("/Home");
			allowAnonymous.Add("/Film/Detail");
            allowAnonymous.Add("/NotFound");

            allowUser.Add("/Film/Booking/Index");
            allowUser.Add("/Transaction/Index");
            allowUser.Add("/Transaction/Payment");
            allowUser.Add("/User/Index");
            allowUser.Add("/User/HistoryBuyTickets");
            allowUser.Add("/User/HistoryRecharges");


            allowAdmin.Add("/Admin/Index");
			allowAdmin.Add("/Admin/Genre/Index");
			allowAdmin.Add("/Admin/Show/Index");
			allowAdmin.Add("/Admin/Show/Detail");
			allowAdmin.Add("/Admin/Show/Create");
            allowAdmin.Add("/Admin/Show/Edit");
            allowAdmin.Add("/Admin/Show/Delete");
            allowAdmin.Add("/Admin/Room/Index");
            allowAdmin.Add("/Admin/Room/Create");
            allowAdmin.Add("/Admin/Booking/Index");
			allowAdmin.Add("/Admin/User/Index");
            allowAdmin.Add("/Admin/User/Edit");
            allowAdmin.Add("/Admin/Film/Index");
            allowAdmin.Add("/Admin/Film/Create");
            allowAdmin.Add("/Admin/Film/Edit");
            allowAdmin.Add("/Admin/HistoryTransaction/Index");
		}


		public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
		{
			string? url = context.HttpContext.Request.RouteValues["page"].ToString();

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
			if (userInfo == null)
			{
				context.Result = new RedirectToPageResult("/SignIn");
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
