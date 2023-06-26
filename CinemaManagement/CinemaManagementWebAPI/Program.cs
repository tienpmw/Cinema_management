using AutoMapper;
using CinemaWebAPI;
using DataAccess.IRepositories;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Data;
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


			//Add Auto Mapper
			var configAutoMapper = new MapperConfiguration(config =>
			{
				config.AddProfile(new AutoMapperProfile());
			});
			var mapper = configAutoMapper.CreateMapper();
			builder.Services.AddSingleton(mapper);
			//Add DI
			//builder.Services.AddSingleton<IRoleRepository, RoleRepository>();
			builder.Services.AddSingleton<ISendMailRepository, SendMailRepository>();

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
					policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
				});
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

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}


		private static IEdmModel GetEdmModel()
		{
			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
			//builder.EntitySet<User>("Users");
			return builder.GetEdmModel();
		}
	}
}