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
        public void CreateFilm(Film film, IFormFile? image)
        {
            FilmDAO.Instance.CreateFilm(film, image);  
        }

        public void UpdateFilm(Film film, IFormFile? image)
        {
            FilmDAO.Instance.UpdateFilm(film, image);
        }
    }
}
