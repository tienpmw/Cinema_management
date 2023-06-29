using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	public class RoleDAO
	{
        //Using Singleton Pattern
        private static RoleDAO instance = null;
        private static readonly object instanceLock = new object();

        public static RoleDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoleDAO();
                    }
                }
                return instance;
            }
        }

        public List<Role> GetAll()
        {
            return new CinemaContext().Role.ToList();
        }
    }
}
