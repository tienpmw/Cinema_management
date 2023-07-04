using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
	public interface IBookingRepository
	{
		void AddBooking(Show show, Booking booking, User user);
		List<Booking> FindBookingByUserId(long idShow, long idUser);
	}
}
