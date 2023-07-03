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

		public void DeleteShow(Show show) => ShowDAO.Instance.DeleteShow(show);

		public List<dynamic> GetEarningAnnual() => ShowDAO.Instance.GetEarningAnnual();

		public long GetEarningMonthly() => ShowDAO.Instance.GetEarningMonthly();

		public Show? GetShowById(long id) => ShowDAO.Instance.GetShowById(id);

		public List<Show> GetShows() => ShowDAO.Instance.GetShows();

		public void UpdateShow(Show show) => ShowDAO.Instance.UpdateShow(show);
	}
}
