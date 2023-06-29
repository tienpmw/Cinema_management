using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
	public class ConfirmEmailRequestDTO
	{
        public string? ConfirmToken { get; set; }
        public string? Email { get; set; }
    }
}
