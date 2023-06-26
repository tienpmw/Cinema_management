using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class Room
	{
		public Room()
		{
			Shows = new List<Show>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int RoomId { get; set; }
		public string? RoomName { get; set; }
		public int NumberRow { get; set; }
		public int NumberColumn { get; set; }

		public virtual ICollection<Show> Shows { get; set; }
	}
}
