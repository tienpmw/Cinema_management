using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class Country
	{
		public Country()
		{
			Films = new List<Film>();
		}

		[Key]
		[StringLength(5)]
		public string? CountryCode{ get; set; }
		public string? CountryName { get; set; }

		public virtual ICollection<Film> Films { get; set; }
	}
}
