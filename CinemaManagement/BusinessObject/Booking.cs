using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class Booking
	{
		[Key]
		public Guid BookingId { get; set; }
		public int ShowId { get; set; }
		public int UserId { get; set; }
		public int SeatBooking { get; set; }
		public decimal Amount { get; set; }
		public DateTime DateBooking { get; set; }
		public string? ContentBill { get; set; }
		public bool IsPay { get; set; }

		[ForeignKey(nameof(ShowId))]
		public virtual Show? Show { get; set; } = null!;
		[ForeignKey(nameof(UserId))]
		public virtual User? User { get; set; } = null!;

	}
}
