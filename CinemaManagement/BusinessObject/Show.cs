using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class Show
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ShowId { get; set; }
		public long RoomId { get; set; }
		public long FilmId { get; set; }
		[Precision(18,2)]
		public decimal Price { get; set; }
		public string? SeatStatus { get; set; }
		public DateTime ShowDate { get; set; }

		[ForeignKey(nameof(RoomId))]
		public virtual Room? Room { get; set; } = null!;
		[ForeignKey(nameof(FilmId))]
		public virtual Film? Film { get; set; } = null!;

	}
}
