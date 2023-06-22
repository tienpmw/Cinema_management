using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CinemaWebClient.Filters
{
    public class AuthorizationUserPageFilter : IAsyncPageFilter
    {

        private List<string> allowAnonymous = new List<string>();
        private List<string> allowUser = new List<string>();


        private void AddRequestAccept()
        {
            allowAnonymous.Add("/Login/Index");
            allowAnonymous.Add("/AccessDenied");
            allowAnonymous.Add("/User/Register");

            allowUser.Add("/Author/Index");
            allowUser.Add("/Book/Index");
            allowUser.Add("/Publisher/Index");
            allowUser.Add("/User/Index");
            allowUser.Add("/Home/Index");
        }

        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            //not check request
            await next.Invoke();
            return;

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
            string? userStr = context.HttpContext.Session.GetString("user");
            if (string.IsNullOrEmpty(userStr))
            {
                context.Result = new RedirectToPageResult("/Login/Index");
                return;
            }

            ///UserDTO? user = JsonConvert.DeserializeObject<UserDTO>(userStr);

            // check request for user's role
            if (2 == 2)
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

            await next.Invoke();
        }

        public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            return Task.CompletedTask;
        }
    }
}
