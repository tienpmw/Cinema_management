using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	public class FilmDAO
	{
        //Using Singleton Pattern
        private static FilmDAO instance = null;
        private static readonly object instanceLock = new object();

        public static FilmDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new FilmDAO();
                    }
                }
                return instance;
            }
        }

        public void CreateFilm(Film film)
        {
            if (IsFilmTitleExisted(film.Title)) throw new Exception("Film's title was existed!");

        }

        public bool IsFilmTitleExisted(string title) 
        {
            return CinemaContext.Instance.Film.FirstOrDefault(x => x.Title == title) != null;   
        }
    }
}
