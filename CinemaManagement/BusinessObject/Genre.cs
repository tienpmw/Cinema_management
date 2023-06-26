using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class Genre
	{
		public Genre()
		{
			Films = new List<Film>();	
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int GenreId { get; set; }
		public string? GenreName { get; set; }

		public virtual ICollection<Film> Films { get; set; }
	}
}
