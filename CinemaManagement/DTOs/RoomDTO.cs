using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class RoomDTO
    {
		public int? RoomId { get; set; }
		[Required(ErrorMessage = "Room's name must be not empty!")]
        [MaxLength(30, ErrorMessage = "Room's name must be less than 30 charactors!")]
        public string? RoomName { get; set; }
        [Required(ErrorMessage = "Room's row chair must be not empty!")]
        [Range(1, 100,ErrorMessage ="Row must be in 1-100!")]
        public int NumberRow { get; set; }
        [Required(ErrorMessage = "Room's colum chair must be not empty!")]
        [Range(1, 100, ErrorMessage = "Column must be in 1-100!")]
        public int NumberColumn { get; set; }
    }
}
