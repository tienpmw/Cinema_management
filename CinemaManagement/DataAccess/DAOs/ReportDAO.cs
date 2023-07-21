using BusinessObject;
using DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	public class ReportDAO
	{
		//Using Singleton Pattern
		private static ReportDAO instance = null;
		private static readonly object instanceLock = new object();

		public static ReportDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new ReportDAO();
					}
				}
				return instance;
			}
		}

		public List<ShowReportDTO> GetShowInMonth(DateTime startDate, DateTime endDate)
		{
			CinemaContext context = new CinemaContext();
			var listShowInRangeTime = context.Show
				.Include(x => x.Room)
				.Include(x => x.Film)
				.Where(x => x.ShowDate >= startDate && x.ShowDate <= endDate)
				.OrderByDescending(x => x.ShowDate)
				.ToList();	

			List<ShowReportDTO> reportData = new List<ShowReportDTO>();
			foreach (var item in listShowInRangeTime) 
			{
				 ShowReportDTO report = new ShowReportDTO()
				 {
					 Index = reportData.Count + 1,
					 Date = item.ShowDate.ToString("dd/MM/yyyy HH:mm"),
					 Title = item.Film.Title,
					 Room = item.Room.RoomName,
					 NumberTicketSold = GetTotalTicketSold(item.SeatStatus),	
					 TotalTicket = item.SeatStatus.Length
				 };	
				 reportData.Add(report);	
			}

			return reportData;
		}

		private int GetTotalTicketSold(string seats)
		{
			var listChar = seats.ToCharArray();
			var total = 0;
			foreach (var item in listChar)
			{
				if (item == '1') total++;
			}
			return total;
		}

		public float GetRevenue(DateTime startDate, DateTime endDate)
		{
			CinemaContext context = new CinemaContext();
			var listShowInRangeTime = context.Show
				.Include(x => x.Room)
				.Include(x => x.Film)
				.Where(x => x.ShowDate >= startDate && x.ShowDate <= endDate)
				.ToList();
			long total = 0;
			foreach (var item in listShowInRangeTime)
			{
				total += item.Price * GetTotalTicketSold(item.SeatStatus);
			}	
			return total;
		}

		public int TotalFilmPublished(DateTime startDate, DateTime endDate)
		{
			CinemaContext context = new CinemaContext();
			var listShowInRangeTime = context.Show
				.Include(x => x.Room)
				.Include(x => x.Film)
				.Where(x => x.ShowDate >= startDate && x.ShowDate <= endDate)
				.ToList();
			List<Film> films = new List<Film>();
			foreach (var item in listShowInRangeTime)
			{
				if(!films.Contains(item.Film)) films.Add(item.Film);
			}

			return films.Count;
		}

		public int TotalShowCreated(DateTime startDate, DateTime endDate)
		{
			CinemaContext context = new CinemaContext();
			var listShowInRangeTime = context.Show
				.Where(x => x.ShowDate >= startDate && x.ShowDate <= endDate)
				.ToList();
			return listShowInRangeTime.Count;	
		}

		public decimal GetPercentageSoldTicketWithTotalTicket(DateTime startDate, DateTime endDate)
		{
			CinemaContext context = new CinemaContext();
			var listShowInRangeTime = context.Show
				.Where(x => x.ShowDate >= startDate && x.ShowDate <= endDate)
				.ToList();
			var totalTicketSold = 0;
			var totalTicket = 0;
			foreach (var item in listShowInRangeTime)
			{
				totalTicketSold += GetTotalTicketSold(item.SeatStatus);
				totalTicket += item.SeatStatus.Length;
			}

			decimal percentage = ((decimal)totalTicketSold / totalTicket) * 100;
			return Math.Round(percentage, 2);
		}

		public long TotalAmountUserRecharge(DateTime startDate, DateTime endDate)
		{
			CinemaContext context = new CinemaContext();
			long total = 0;
			var transactions = context.Transaction
				.Where(x => x.PaidDate >= startDate && x.PaidDate <= endDate && x.IsPay == true)
				.ToList();
			foreach (var item in transactions)
			{
				total += item.Amount;
			}
			return total;
		}
	}
}
