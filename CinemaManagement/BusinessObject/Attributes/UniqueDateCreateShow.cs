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

		public override string FormatErrorMessage(string name)
		{
			return string.Format(CultureInfo.InvariantCulture, ErrorMessage, name);
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
			long duration = film.FilmDuration;
			DateTime date = (DateTime)value;
			if (DateTime.Compare(date, film.DateRelease) < 0) return new ValidationResult($"Choose date greater {film.DateRelease.ToString("dd/MM/yyyy hh:mm tt")}.");
			DateTime dateBefore = date.AddMinutes(-1 * duration);
			DateTime dateAfter = date.AddMinutes(duration);
			bool isExisted = context.Show.Any(x => x.RoomId == roomId && x.FilmId == filmId && x.ShowDate >= dateBefore && x.ShowDate <= dateAfter);
			if (isExisted) return new ValidationResult($"Let's choose other date.");
			return ValidationResult.Success;
		}
	}
}
