using BusinessObject;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ShowDTO
    {
        public long ShowId { get; set; }
        public long RoomId { get; set; }
        public long FilmId { get; set; }
        public long Price { get; set; }
        public string? SeatStatus { get; set; }
        public DateTime ShowDate { get; set; }
        public virtual RoomDTO? Room { get; set; } = null!;
        public virtual FilmDTO? Film { get; set; } = null!;
    }
}
