﻿using BusinessObject;
using DataAccess.DAOs;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ReportDataController : ControllerBase
	{
		[HttpPost("GetReportCompanyInRangeTime")]
		public IActionResult GetReportCompanyInRangeTime([FromForm] DateTime startDate, [FromForm] DateTime endDate)
		{
			if (startDate == new DateTime() || endDate == new DateTime())
			{
				return Conflict("Date cannot be empty!");
			}
			if (DateTime.Now < startDate)
			{
				return Conflict("You must be choose end time less than current time!");
			}
			if (endDate < startDate)
			{
				return Conflict("Chose again date");
			}

			ReportCompanyInRangeTimeRespone report = new ReportCompanyInRangeTimeRespone()
			{
				Shows = ReportDAO.Instance.GetShowInMonth(startDate, endDate),
				Revenue = ReportDAO.Instance.GetRevenue(startDate, endDate),
				TotalFilmPublished = ReportDAO.Instance.TotalShowCreated(startDate, endDate),
				TotalShowCreated = ReportDAO.Instance.TotalShowCreated(startDate, endDate),
				PercentageSoldTicketWithTotalTicket = ReportDAO.Instance.GetPercentageSoldTicketWithTotalTicket(startDate, endDate),
				TotalAmountUserRecharge = ReportDAO.Instance.TotalAmountUserRecharge(startDate, endDate)
			};
			return Ok(report);
		}
	}

	public class ReportCompanyInRangeTimeRespone
	{
		public List<ShowReportDTO> Shows { get; set; }
		public float Revenue { get; set; }	
		public float TotalFilmPublished { get; set; }
		public int TotalShowCreated { get; set; }
		public decimal PercentageSoldTicketWithTotalTicket { get; set; }	
		public long TotalAmountUserRecharge { get; set; }	
	}
}

