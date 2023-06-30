using BusinessObject.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ShowCreateDTO
    {
        [Required]
        public long RoomId { get; set; }
        [Required]
        public long FilmId { get; set; }
        [Required]
        public long Price { get; set; }
        [UniqueDateCreateShow]
        public DateTime ShowDate { get; set; }
    }
}
