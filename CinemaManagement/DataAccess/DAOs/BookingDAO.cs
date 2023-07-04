using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DataAccess.DAOs
{
	public class BookingDAO
	{
		//Using Singleton Pattern
		private static BookingDAO instance = null;
		private static readonly object instanceLock = new object();

		public static BookingDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new BookingDAO();
					}
				}
				return instance;
			}
		}

		public void AddBooking(Show show, Booking booking, User user)
		{
			using (var context = new CinemaContext())
			{
				using (var transaction = context.Database.BeginTransaction())
				{
					try
					{
						context.Booking.Add(booking);
						context.User.Update(user);
						context.Show.Update(show);
						context.SaveChanges();
						transaction.Commit();
					}
					catch (Exception)
					{
						transaction.Rollback();
						throw new Exception();
					}
				}
			}
		}

		public List<Booking> FindBookingByUserId(long idShow, long idUser)
		{
			return new CinemaContext().Booking.Where(x => x.ShowId == idShow && x.UserId == idUser).OrderBy(x => x.DateBooking).ToList();
		}
	}
}
