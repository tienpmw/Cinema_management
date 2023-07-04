using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
	public class BookingRepository : IBookingRepository
	{
		public void AddBooking(Show show, Booking booking, User user) => BookingDAO.Instance.AddBooking(show, booking, user);

		public List<Booking> FindBookingByUserId(long idShow, long idUser) => BookingDAO.Instance.FindBookingByUserId(idShow, idUser);
	}
}
