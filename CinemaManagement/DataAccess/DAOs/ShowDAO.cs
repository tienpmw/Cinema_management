using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
