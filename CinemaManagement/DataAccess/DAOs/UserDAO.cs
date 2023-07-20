using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	public class UserDAO
	{
		private static UserDAO instance = null;
		private static readonly object instanceLock = new object();

		public static UserDAO Instance
		{
			get
			{
				lock (instanceLock)
				{
					if (instance == null)
					{
						instance = new UserDAO();
					}
				}
				return instance;
			}
		}

		public void AddUser(User user)
		{
			try
			{
				CinemaContext context = new CinemaContext();
				context.User.Add(user);
				context.SaveChanges();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}

		}

		public User? GetUserByEmail(string email)
		{
			return new CinemaContext().User.Include(x => x.Role).FirstOrDefault(x => x.Email == email);
		}

		public User? GetUserById(long userId)
		{
			return new CinemaContext().User.Include(x => x.Role).FirstOrDefault(x => x.UserId == userId);
		}

		public void UpdateConfirmEmail(string email)
		{
			try
			{
				var context = new CinemaContext();
				User? user = GetUserByEmail(email);
				user.IsConfirmEmail = true;
				context.Update(user);
				context.SaveChanges();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
			
		}

		public List<User> GetAll()
		{
			var context = new CinemaContext();
			return context.User.Include(x => x.Role).ToList();
		}

		public void Update(User user)
		{
			try
			{
				CinemaContext context = new CinemaContext();
				context.Entry(user).State = EntityState.Modified;
				context.SaveChanges();
			}
			catch (Exception ex)
			{

				throw new Exception(ex.Message);
			}
		}

        public List<Booking> GetAllSeatBookedByUserId(long id)
        {
            return new CinemaContext().Booking.Include(x => x.Show).Include(x => x.Show.Film).Where(x => x.UserId == id).OrderByDescending(x => x.DateBooking).ToList();
        }
    }
}
