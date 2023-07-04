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

       

        public List<User> GetAll() => UserDAO.Instance.GetAll();

        public User? GetUserByEmail(string email) => UserDAO.Instance.GetUserByEmail(email);

        public User? GetUserById(long userId) => UserDAO.Instance.GetUserById(userId);

        public void Update(User user) => UserDAO.Instance.Update(user);

		public void UpdateConfirmEmail(string email) => UserDAO.Instance.UpdateConfirmEmail(email);

        public List<Booking> GetAllSeatBookedByUserId(int id) => UserDAO.Instance.GetAllSeatBookedByUserId(id);


    }
}
