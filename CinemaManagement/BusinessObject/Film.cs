using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class Film
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long FilmId { get; set; }
		public long GenreId { get; set; }
		public string? CountryCode { get; set; }
		public string? Title { get; set; }
		[DataType(DataType.MultilineText)]
		public string? Description { get; set; }
		public string? Image { get; set; }
		public long FilmDuration { get; set; }
		public DateTime DateRelease { get; set; }
		[ForeignKey(nameof(GenreId))]
		public virtual Genre? Genre { get; set; } = null!;
		[ForeignKey(nameof(CountryCode))]
		public virtual Country? Country { get; set; } = null!;


	}
}
