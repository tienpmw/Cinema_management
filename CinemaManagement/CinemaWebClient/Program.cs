using CinemaWebClient.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CinemaWebClient
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			// Add services to the container.
			builder.Services.AddRazorPages();

			// Add start up page
			builder.Services.AddMvc()
				.AddRazorPagesOptions(options =>
				{
					options.Conventions.AddPageRoute("/Home", "");
				});

			// add session
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(60);
				options.Cookie.HttpOnly = true;
			});

			builder.Services.AddMvc(options =>
			{
				options.Filters.Add(new AuthorizationUserPageFilter());
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
			}

			//Handle 404 not found page
			app.Use(async (context, next) =>
			{
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/NotFound";
                    await next();
                }
            });

			app.UseSession();

			app.UseStaticFiles();

			app.UseRouting();
			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
	}
}