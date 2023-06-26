﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class User
	{
		public User()
		{
			Bookings = new List<Booking>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int UserId { get; set; }
		public int RoleId { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public bool IsActive { get; set; }
		public bool IsConfirmEmail { get; set; }
		public bool IsLoginGoogle { get; set; }

		[ForeignKey(nameof(RoleId))]
		public virtual Role? Role { get; set; } = null!;
		public virtual ICollection<Booking> Bookings { get; set; }

	}
}
