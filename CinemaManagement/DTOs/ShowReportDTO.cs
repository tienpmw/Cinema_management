using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
	public class ShowReportDTO
	{
		[Description("STT")]
		public int Index { get; set; }
		[Description("Thời gian chiếu")]
		public string Date { get; set; }
		[Description("Tên phim")]
		public string Title { get; set; }
		[Description("Tên phòng")]
		public string Room { get; set; }
		[Description("Số vé đã được bán")]
		public int NumberTicketSold { get; set; }
		[Description("Tổng số vé")]
		public int TotalTicket { get; set; }
	}
}
