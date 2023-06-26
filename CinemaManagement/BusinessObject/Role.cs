using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class Role
	{
		public Role()
		{
			Users = new List<User>();
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long RoleId { get; set; }
		public string? RoleName { get; set; }

		public virtual ICollection<User> Users { get; set;}
	}
}
