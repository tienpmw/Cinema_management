using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
	public class RefreshToken
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long Id { get; set; }	
		public long UserId { get; set; }

		public string? TokenRefresh { get; set; }
		public string? JwtId { get; set; }
		public bool IsUsed { get; set; }
		public bool IsRevoked { get; set; }

		[ForeignKey(nameof(UserId))]
		public virtual User? User { get; set; } = null!;
	}
}
