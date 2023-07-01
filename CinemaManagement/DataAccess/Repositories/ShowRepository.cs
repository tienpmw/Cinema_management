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
    public class ShowRepository : IShowRepository
    {
		public void AddShow(Show show) => ShowDAO.Instance.AddShow(show);

		public List<Show> GetShows() => ShowDAO.Instance.GetShows();
    }
}
