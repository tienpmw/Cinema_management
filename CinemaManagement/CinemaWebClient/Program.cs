using CinemaWebClient.Filters;

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
                    options.Conventions.AddPageRoute("/Login/Index", "");
                });

            // add session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(300);
                options.Cookie.HttpOnly = true;
            });

            // add filter request
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

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}