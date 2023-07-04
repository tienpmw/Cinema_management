using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class BookingDTO
    {
        public Guid BookingId { get; set; }
        public string? SeatBooking { get; set; }
        public long Amount { get; set; }
        public DateTime DateBooking { get; set; }
        public string? ContentBill { get; set; }
        public bool IsPay { get; set; }
        public virtual Show? Show { get; set; } = null!;
    }
}
