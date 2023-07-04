using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Attributes
{
	public class UniqueDateCreateShow : ValidationAttribute
	{
		public UniqueDateCreateShow()
		{
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			CinemaContext context = new CinemaContext();
			dynamic instance = validationContext.ObjectInstance;
			if (instance.FilmId == null || instance.RoomId == null) return new ValidationResult("Select Film and Room before choose show date.");
			int? filmId = (int)instance.FilmId;
			int? roomId = (int)instance.RoomId;
			if (value == null) return new ValidationResult("Show date not blank.");
			Film? film = context.Film.FirstOrDefault(x => x.FilmId == filmId);
			if (film == null) return new ValidationResult("");
			long duration = film.FilmDuration;
			DateTime date = (DateTime)value;
			if (date.Subtract(DateTime.Now) < new TimeSpan(2, 0, 0)) return new ValidationResult("Please choose date with gap greater than 2 hours of current time");
			if (DateTime.Compare(date, new DateTime(date.Year, date.Month, date.Day, 8, 0, 0)) < 0 || DateTime.Compare(date, new DateTime(date.Year, date.Month, date.Day, 20, 0, 0)) > 0)
			{
				return new ValidationResult("Please choose time in range 08:00:00 AM - 08:00:00 PM.");
			}
			if (DateTime.Compare(date, film.DateRelease) < 0) return new ValidationResult($"Choose date greater {film.DateRelease.ToString("dd/MM/yyyy hh:mm tt")}.");
			DateTime dateBefore = date.AddMinutes(-1 * duration);
			DateTime dateAfter = date.AddMinutes(duration);
			//bool isExisted = context.Show.Any(x => x.RoomId == roomId && x.FilmId == filmId && x.ShowDate >= dateBefore && x.ShowDate <= dateAfter);
			bool isExisted = context.Show.Include(x => x.Film).Any(x => x.RoomId == roomId 
			&& x.ShowDate.Date == date.Date
			&& (x.ShowDate.AddMinutes(x.Film.FilmDuration) > date && x.ShowDate <= dateAfter));
			if (isExisted) return new ValidationResult($"Let's choose other date.");
			return ValidationResult.Success;
		}
	}
}
