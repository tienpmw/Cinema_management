using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class FilmRepository : IFilmRepository
    {
		public long CountFilm() => FilmDAO.Instance.CountFilm();

		public void CreateFilm(Film film, IFormFile? image)
        {
            FilmDAO.Instance.CreateFilm(film, image);  
        }

        public List<Film> GetAll() => FilmDAO.Instance.GetAll();

        public Film? GetFilmById(long id) => FilmDAO.Instance.GetFilmById(id);

		public void UpdateFilm(Film film, IFormFile? image)
        {
            FilmDAO.Instance.UpdateFilm(film, image);
        }
    }
}
