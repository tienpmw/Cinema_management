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
		User? GetUserByEmail(string email);
		User? GetUserById(long userId);
		void UpdateConfirmEmail(string email);
	}
}
