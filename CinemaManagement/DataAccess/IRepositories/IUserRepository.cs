using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
	public interface IUserRepository
	{
		void AddUser(User userSignIn);
        List<User> GetAll();
        User? GetUserByEmail(string email);
		User? GetUserById(long userId);
		void Update(User user);
		void UpdateConfirmEmail(string email);
        List<Booking> GetAllSeatBookedByUserId(int id);
	}
}
