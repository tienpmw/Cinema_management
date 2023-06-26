using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class CInemaContext : DbContext
	{
		public CInemaContext(DbContextOptions options) : base(options)
		{
		}
		public virtual DbSet<Role> Role { get; set; } = null!;
		public virtual DbSet<User> User { get; set; } = null!;
		public virtual DbSet<Booking> Booking { get; set; } = null!;
		public virtual DbSet<Show> Show { get; set; } = null!;
		public virtual DbSet<Room> Room { get; set; } = null!;
		public virtual DbSet<Film> Film { get; set; } = null!;
		public virtual DbSet<Country> Country { get; set; } = null!;
		public virtual DbSet<Genre> Genre { get; set; } = null!;

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			var directory = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().LastIndexOf("/")) + "/CinemaWebAPI";
			var conf = new ConfigurationBuilder()
				.SetBasePath(directory)
				.AddJsonFile("appsettings.json", true, true)
				.Build();
			if (!optionsBuilder.IsConfigured)
			{
				optionsBuilder.UseSqlServer(conf.GetConnectionString("DB"));
			}
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Role>().HasData(new Role[]
			{
				new Role{RoleId = 1, RoleName="Admin"},
				new Role{RoleId = 2, RoleName="User"}
			});
		}
	}
}
