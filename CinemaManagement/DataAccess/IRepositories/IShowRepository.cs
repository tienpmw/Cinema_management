using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
    public interface IShowRepository
    {
		void AddShow(Show show);
		void DeleteShow(Show show);
		List<dynamic> GetEarningAnnual();
		long GetEarningMonthly();
		Show? GetShowById(long id);
		List<Show> GetShows();
		void UpdateShow(Show show);
	}
}
