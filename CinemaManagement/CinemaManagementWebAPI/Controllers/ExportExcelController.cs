using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using OfficeOpenXml.Table;
using OfficeOpenXml;
using System.Drawing;
using DataAccess.DAOs;
using System.Reflection;

namespace CinemaWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ExportExcelController : ControllerBase
	{
		public ExportExcelController() 
		{

		}

		[HttpPost("DataShows")]
		public IActionResult ExportDataShows([FromForm] DateTime startDate, [FromForm] DateTime endDate)
		{
			if(startDate == new DateTime() || endDate == new DateTime())
			{
				return Conflict("Date cannot be empty!");
			}
			if(DateTime.Now < startDate) 
			{
				return Conflict("You must be choose end time less than current time!");
			}
			if(endDate < startDate) 
			{
				return Conflict("Chose again date");
			}

			string filename = $"Bang_Thong_Ke_{startDate.Day}/{startDate.Month}/{startDate.Year}-{endDate.Day}/{endDate.Month}/{endDate.Year}_{Guid.NewGuid():N}.xlsx";
			using ExcelPackage pack = new ExcelPackage();
			ExcelWorksheet excel = pack.Workbook.Worksheets.Add(filename);

			// config all data in file
			excel.Cells.Style.Font.Name = "Segoe UI Historic";
			excel.Columns[1].Width = 6; //A
			excel.Columns[2].Width = 20; //B
			excel.Columns[3].Width = 45; //C
			excel.Columns[4].Width = 35; //D
			excel.Columns[5].Width = 25; //E
			excel.Columns[6].Width = 15; //F


			//Reference Comany
			var positionCompanyName = "A1";
			excel.Rows[1].Height = 24;
			excel.Cells[positionCompanyName].Value = "Công Ty HieuLD6-TienPM7 Cinema";
			excel.Cells[positionCompanyName].Style.Font.Bold = true;
			excel.Cells[positionCompanyName].Style.Font.Size = 12;

			//Header
			var positionHeader = "C4";
			excel.Rows[4].Height = 30;
			excel.Cells[positionHeader].Value = $"BẢNG BÁO CÁO DỮ LIỆU {startDate.Day}/{startDate.Month}/{startDate.Year} - {endDate.Day}/{endDate.Month}/{endDate.Year}";
			excel.Cells[positionHeader].Style.Font.Bold = true;
			excel.Cells[positionHeader].Style.Font.Size = 18;

			//I. data films was shows
			var positionHeaderPart1 = "A7";
			excel.Rows[1].Height = 24;
			excel.Cells[positionHeaderPart1].Value = "I. Danh sách các phim đã được chiếu:";
			excel.Cells[positionHeaderPart1].Style.Font.Bold = true;
			excel.Cells[positionHeaderPart1].Style.Font.Size = 11;

			var positionTable = "A8";
			var table = ReportDAO.Instance.GetShowInMonth(startDate, endDate);
			excel.Cells[positionTable].LoadFromCollection(table, true, TableStyles.Light1);

			//II. Statistics data
			var marginTop = 3;
			var startPositionPart2 = 7 + table.Count + marginTop;
			var indexPart2 = 0;
			//header
			var postionHeaderPart2 = "A" + startPositionPart2;
			excel.Cells[postionHeaderPart2].Value = "II. Số liệu thống kê khác:";
			excel.Cells[postionHeaderPart2].Style.Font.Bold = true;
			excel.Cells[postionHeaderPart2].Style.Font.Size = 11;
			indexPart2++;
			//1.
			excel.Cells["A" + (startPositionPart2 + indexPart2)].Value = "1.Doanh thu: " + ReportDAO.Instance.GetRevenue(startDate, endDate) + "VND";
			indexPart2++;
			//2.
			excel.Cells["A" + (startPositionPart2 + indexPart2)].Value = "2.Số phim đã được chiếu: " + ReportDAO.Instance.TotalFilmPublished(startDate,endDate);
			indexPart2++;
			//3.
			excel.Cells["A" + (startPositionPart2 + indexPart2)].Value = "3.Số phòng chiếu đã được tạo: " + ReportDAO.Instance.TotalShowCreated(startDate, endDate);
			indexPart2++;
			//3.
			excel.Cells["A" + (startPositionPart2 + indexPart2)].Value = "4.Tỉ lệ số ghế đã bán / tổng số ghế bỏ trống: " + ReportDAO.Instance.GetPercentageSoldTicketWithTotalTicket(startDate, endDate) + "%"; 
			indexPart2++;
			//4.
			excel.Cells["A" + (startPositionPart2 + indexPart2)].Value = "5. Tổng số tiền người dùng đã nạp vào ứng dụng: " + ReportDAO.Instance.TotalAmountUserRecharge(startDate, endDate) + "VND"; 
			indexPart2++;
			//5.
			excel.Cells["A" + (startPositionPart2 + indexPart2)].Value = "6. Số lượng người dùng mới :";
			indexPart2++;

			var exportbytes = pack.GetAsByteArray();
			return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
		}
	}
}
