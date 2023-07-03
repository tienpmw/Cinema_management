using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	public class ShowDAO
	{
		//Using Singleton Pattern
		private static ShowDAO instance = null;
		private static readonly object instanceLock = new object();

		public static ShowDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new ShowDAO();
					}
				}
				return instance;
			}
		}

		public List<Show> GetShows()
		{
			return new CinemaContext().Show.Include(x => x.Film).Include(x => x.Room).ToList();
		}

		public void AddShow(Show show)
		{

			try
			{
				var context = new CinemaContext();
				Film? film = context.Film.FirstOrDefault(x => x.FilmId == show.FilmId);
				Room? room = context.Room.FirstOrDefault(x => x.RoomId == show.RoomId);
				if (film == null || room == null) throw new Exception();
				if (DateTime.Compare(show.ShowDate, new DateTime(show.ShowDate.Year, show.ShowDate.Month, show.ShowDate.Day, 8, 0, 0)) < 0
					|| DateTime.Compare(show.ShowDate, new DateTime(show.ShowDate.Year, show.ShowDate.Month, show.ShowDate.Day, 20, 0, 0)) > 0)
				{
					throw new Exception();
				}
				if (DateTime.Compare(show.ShowDate, film.DateRelease) < 0) throw new Exception();
				DateTime dateBefore = show.ShowDate.AddMinutes(-1 * film.FilmDuration);
				DateTime dateAfter = show.ShowDate.AddMinutes(film.FilmDuration);
				bool isAnyShow = context.Show.Any(x => x.RoomId == show.RoomId && x.FilmId == show.FilmId && x.ShowDate >= dateBefore && x.ShowDate <= dateAfter);
				if (isAnyShow) throw new Exception();
				show.SeatStatus = new string('0', room.NumberColumn * room.NumberRow);
				context.Show.Add(show);
				context.SaveChanges();
			}
			catch (Exception)
			{

				throw new Exception();
			}
		}

		public long GetEarningMonthly()
		{
			var context = new CinemaContext();
			var result = from b in context.Booking
						 where b.DateBooking.Year == DateTime.Now.Year && b.DateBooking.Month == DateTime.Now.Month && b.IsPay == true
						 select b;
			return result.Sum(x => x.Amount);
		}

		public List<dynamic> GetEarningAnnual()
		{
			var context = new CinemaContext();
			List<dynamic> list = new List<dynamic>()
			{
				new
				{
					Month = 1,
					Earning = 0
				},
				new
				{
					Month = 2,
					Earning = 0
				},
				new
				{
					Month = 3,
					Earning = 0
				},
				new
				{
					Month = 4,
					Earning = 0
				},
				new
				{
					Month = 5,
					Earning = 0
				},
				new
				{
					Month = 6,
					Earning = 0
				},
				new
				{
					Month = 7,
					Earning = 0
				},
				new
				{
					Month = 8,
					Earning = 0
				},
				new
				{
					Month = 9,
					Earning = 0
				},
				new
				{
					Month = 10,
					Earning = 0
				},
				new
				{
					Month = 11,
					Earning = 0
				},
				new
				{
					Month = 12,
					Earning = 0
				},
			};
            List<dynamic> lsEarningByMonth = (from b in context.Booking
								   where b.DateBooking.Year == DateTime.Now.Year && b.DateBooking.Month == DateTime.Now.Month && b.IsPay == true
								   group b by new { b.DateBooking.Month } into tb1
								   select new
								   {
									   Month = tb1.Key.Month,
									   Earning = tb1.Sum(x => x.Amount)
								   }).ToList<dynamic>();
			if (lsEarningByMonth.Count() == 0)
			{
				return list;
			}

			List < dynamic > result = (from l in list
									   join e in lsEarningByMonth
									   on l.Month equals e.Month into tb2
									   from tb3 in tb2.DefaultIfEmpty()
									   select new
									   {
										   Month = l.Month,
										   Earning = tb3?.Earning ?? 0
									   }).ToList<dynamic>();
			return result;
		}

		public Show? GetShowById(long id)
		{
			return new CinemaContext().Show.Include(x => x.Room).Include(x => x.Film).FirstOrDefault(x => x.ShowId == id);
		}

		public void UpdateShow(Show show)
		{
			try
			{
				var context = new CinemaContext();
				Show? show1 = context.Show.FirstOrDefault(x => x.ShowId == show.ShowId);
				if (show1 == null) throw new Exception();
				Film? film = context.Film.FirstOrDefault(x => x.FilmId == show1.FilmId);
				Room? room = context.Room.FirstOrDefault(x => x.RoomId == show1.RoomId);
				if (film == null || room == null) throw new Exception();
				if (DateTime.Compare(show.ShowDate, new DateTime(show.ShowDate.Year, show.ShowDate.Month, show.ShowDate.Day, 8, 0, 0)) < 0
					|| DateTime.Compare(show.ShowDate, new DateTime(show.ShowDate.Year, show.ShowDate.Month, show.ShowDate.Day, 20, 0, 0)) > 0)
				{
					throw new Exception();
				}
				if (DateTime.Compare(show.ShowDate, film.DateRelease) < 0) throw new Exception();
				DateTime dateBefore = show.ShowDate.AddMinutes(-1 * film.FilmDuration);
				DateTime dateAfter = show.ShowDate.AddMinutes(film.FilmDuration);
				bool isAnyShow = context.Show.Any(x => x.RoomId == show.RoomId && x.FilmId == show.FilmId && x.ShowDate >= dateBefore && x.ShowDate <= dateAfter && x.ShowId != show.ShowId);
				if (isAnyShow) throw new Exception();
				show1.ShowDate = show.ShowDate;
				context.Show.Update(show1);
				context.SaveChanges();
			}
			catch (Exception)
			{
				throw new Exception();
			}
		}

		public void DeleteShow(Show show)
		{
			try
			{
				var context = new CinemaContext();
				List<Booking> bookings = context.Booking.Where(x => x.ShowId == show.ShowId).ToList();
				if (bookings.Count > 0)
				{
					foreach (var b in bookings)
					{
						User? user = context.User.FirstOrDefault(x => x.UserId == b.UserId);
						user.AccountBalance += b.Amount;
						context.User.Update(user);
					}
					context.Booking.RemoveRange(bookings);
				}
				context.Show.Remove(show);
				context.SaveChanges();
			}
			catch (Exception)
			{

				throw new Exception();
			}
		}
	}
}
