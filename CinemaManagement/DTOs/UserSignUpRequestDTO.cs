using BusinessObject.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
	public class UserSignUpRequestDTO
	{
		[Required(ErrorMessage = "Email not blank!")]
		[DataType(DataType.EmailAddress)]
		[UniqueEmail(ErrorMessage = "Email was used!")]
		public string? Email { get; set; }
		[Required(ErrorMessage = "Password not blank!")]
		public string? Password { get; set; }
		[Required(ErrorMessage = "Confirm password not blank!")]
		[Compare(nameof(Password), ErrorMessage = "Confirm password not match with Password!")]
		public string? ConfirmPassword { get; set; }
		[Required(ErrorMessage = "Firstname not blank!")]
		[MaxLength(50, ErrorMessage = "Firstname is maximum 50 character!")]
		public string? FirstName { get; set; }
		[Required(ErrorMessage = "Lastname not blank!")]
		[MaxLength(50, ErrorMessage = "Lastname is maximum 50 character!")]
		public string? LastName { get; set; }
	}
}
