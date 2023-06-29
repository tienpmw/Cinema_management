using BusinessObject;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Attributes
{
	public class UniqueEmail : ValidationAttribute
	{
		public UniqueEmail()
		{
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
		{
			if (value == null) return ValidationResult.Success;
			string email = value as string;
			CinemaContext context = new CinemaContext();
			if (context.User.Any(x => x.Email.ToLower() == email.Trim().ToLower()))
			{
				return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
			}
			return ValidationResult.Success;
		}

		public override string FormatErrorMessage(string name)
		{
			return string.Format(CultureInfo.InvariantCulture, ErrorMessage, name);
		}
	}
}
