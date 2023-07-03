using BusinessObject;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.IRepositories
{
	public interface IFilmRepository
	{
		void CreateFilm(Film film, IFormFile? image);
		void UpdateFilm(Film film, IFormFile? image);
		Film? GetFilmById(long id);
		List<Film> GetAll();
		long CountFilm();
	}
}
