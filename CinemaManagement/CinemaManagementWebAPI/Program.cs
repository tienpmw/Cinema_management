using AutoMapper;
using BusinessObject;
using CinemaWebAPI;
using CinemaWebAPI.Jobs;
using CinemaWebAPI.Policies;
using CinemaWebAPI.Utilities;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Quartz;
using System.Data;
using System.Text;
using System.Text.Json.Serialization;

namespace CinemaManagementWebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// Add database
			builder.Services.AddDbContext<CinemaContext>(opts =>
			{
				opts.UseSqlServer(builder.Configuration.GetConnectionString("DB"));
			});

			// add job scheduler get history transaction bank
			builder.Services.AddQuartz(q =>
			{
				q.UseMicrosoftDependencyInjectionScopedJobFactory();
				var jobKeyGetHistoryTransaction = new JobKey("GetHistoryTransactionJob");
				q.AddJob<HistoryTransactionMbBankJob>(opts => opts.WithIdentity(jobKeyGetHistoryTransaction));
				q.AddTrigger(opts => opts
					.ForJob(jobKeyGetHistoryTransaction)
					.StartNow()
					.WithSimpleSchedule(x =>
						x.WithIntervalInMinutes(1)
						.RepeatForever()
						)
					);
				var jobKeyRemoveExpiredTransaction = new JobKey("RemoveExpiredTransaction");
				q.AddJob<RemoveExpiredTransactionJob>(opts => opts.WithIdentity(jobKeyRemoveExpiredTransaction));
                q.AddTrigger(opts => opts
                    .ForJob(jobKeyRemoveExpiredTransaction)
                    .StartNow()
                    .WithSimpleSchedule(x =>
                        x.WithIntervalInHours(24)
                        .RepeatForever()
                        )
                    );
            });

            builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);



            //Add Auto Mapper
            var configAutoMapper = new MapperConfiguration(config =>
			{
				config.AddProfile(new AutoMapperProfile());
			});
			var mapper = configAutoMapper.CreateMapper();
			builder.Services.AddSingleton(mapper);
			//Add DI
			builder.Services.AddSingleton<IRoleRepository, RoleRepository>();
			builder.Services.AddSingleton<IFilmRepository, FilmRepository>();
			builder.Services.AddSingleton<IShowRepository, ShowRepository>();
			builder.Services.AddSingleton<IUserRepository, UserRepository>();
			builder.Services.AddSingleton<IBookingRepository, BookingRepository>();
			builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
			builder.Services.AddSingleton<IGenreRepository, GenreRepository>();
			builder.Services.AddSingleton<ICountryRepository, CountryRepository>();
			builder.Services.AddSingleton<ISendMailRepository, SendMailRepository>();
			builder.Services.AddSingleton<ITransactionRepository, TransactionRepository>();
			builder.Services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();

			//Add Odata
			builder.Services.AddControllers()
				.AddOData(options =>
				{
					options.Select().Filter().Count().OrderBy().SetMaxTop(100).Expand()
					.AddRouteComponents("odata", GetEdmModel());
				});
			//Add Cors
			builder.Services.AddCors(options =>
			{
				options.AddDefaultPolicy(policy =>
				{
					policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
				});
			});
			//Add JWT
			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(opt =>
				{
					opt.TokenValidationParameters = new TokenValidationParameters
					{
						//tự cấp token
						ValidateIssuer = false,
						ValidateAudience = false,

						//ký vào token
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),

						ClockSkew = TimeSpan.Zero // using for validate token
					};
				});

			// Add authorization policy
			builder.Services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
			builder.Services.AddAuthorization(opts =>
			{
				opts.AddPolicy("Permission", policy =>
				{
					policy.AddRequirements(new PermissionRequirement());
					policy.RequireAuthenticatedUser()
					.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
				});
				opts.DefaultPolicy = opts.GetPolicy("Permission"); // set default policy
			});
			// Remove cycle object's data in json respone
			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
			});
			// Disable auto model state validate
			builder.Services.Configure<ApiBehaviorOptions>(opts =>
			{
				opts.SuppressModelStateInvalidFilter = true;
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors();

			app.UseAuthentication();

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}


		private static IEdmModel GetEdmModel()
		{
			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
			builder.EntitySet<User>("Users");
            builder.EntitySet<Transaction>("Transactions");
            builder.EntitySet<Genre>("Genre");
            builder.EntitySet<Film>("Film");
            return builder.GetEdmModel();
		}
	}
}