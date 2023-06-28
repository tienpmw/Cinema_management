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
    public class UserRepository : IUserRepository
    {
        public void AddUser(User user) => UserDAO.Instance.AddUser(user);

        public User? GetUserByEmail(string email) => UserDAO.Instance.GetUserByEmail(email);

        public User? GetUserById(long userId) => UserDAO.Instance.GetUserById(userId);

        public void UpdateConfirmEmail(string email) => UserDAO.Instance.UpdateConfirmEmail(email);
	}
}
