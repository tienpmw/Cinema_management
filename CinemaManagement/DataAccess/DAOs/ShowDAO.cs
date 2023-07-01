using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

		public void AddShow(Show show)
		{

			try
			{
				var context = new CinemaContext();
				Film? film = context.Film.FirstOrDefault(x => x.FilmId == show.FilmId);
				Room? room = context.Room.FirstOrDefault(x => x.RoomId == show.RoomId);
				if (film == null || room == null) throw new Exception();
				if (DateTime.Compare(show.ShowDate, film.DateRelease) < 0) throw new Exception();
				DateTime dateBefore = show.ShowDate.AddMinutes(-1 * film.FilmDuration);
				DateTime dateAfter = show.ShowDate.AddMinutes(film.FilmDuration);
				bool isAnyShow = context.Show.Any(x => x.RoomId == show.RoomId && x.FilmId == show.FilmId && x.ShowDate >= dateBefore && x.ShowDate <= dateAfter);
				if (isAnyShow) throw new Exception();
				show.SeatStatus = new string('0', room.NumberColumn * room.NumberRow);
				context.Show.Add(show);
				context.SaveChanges();
			}
			catch (Exception)
			{

				throw new Exception();
			}
		}
	}
}
