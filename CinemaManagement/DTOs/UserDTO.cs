using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
	public class UserDTO
	{
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsConfirmEmail { get; set; }
        public bool IsLoginGoogle { get; set; }
        public long AccountBalance { get; set; }
    }
}
