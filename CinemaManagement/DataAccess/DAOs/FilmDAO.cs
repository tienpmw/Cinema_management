using BusinessObject;
using DataAccess.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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

		public void CreateFilm(Film film, IFormFile? image)
		{
			CinemaContext cinemaContext = new CinemaContext();
			if (IsFilmTitleExisted(film.Title)) throw new Exception("Film's title was existed!");
			Util util = new Util();
			string pathFolder = "\\Data\\Images";
			string pathSaveImage = Directory.GetCurrentDirectory() + pathFolder;
			string fileName = Guid.NewGuid().ToString() + image.FileName.Substring(image.FileName.LastIndexOf("."));
			util.SaveFile(image, pathSaveImage, fileName);
			film.DateRelease = DateTime.Now;
			film.Image = fileName;
			cinemaContext.Film.Add(film);
			cinemaContext.SaveChanges();
		}

		public void UpdateFilm(Film film, IFormFile? image)
		{
			CinemaContext cinemaContext = new CinemaContext();
			var filmUpdate = cinemaContext.Film.First(x => x.FilmId == film.FilmId);

			if (GetFilmById(film.FilmId) == null) throw new Exception("Film was not existed!");
			if (IsFilmTitleExisted(film.Title) && film.Title != filmUpdate.Title) throw new Exception("Film's title was existed!");

			// change image
			if (image != null)
			{
				Util util = new Util();
				string pathFolder = "\\Data\\Images";
				string pathFolderImage = Directory.GetCurrentDirectory() + pathFolder;
				string fileNameNewImage = Guid.NewGuid().ToString() + image.FileName.Substring(image.FileName.LastIndexOf("."));
				util.SaveFile(image, pathFolderImage, fileNameNewImage);
				// remove old image
				util.DeleteFile(image, pathFolderImage, film.Image);

				// update file name img of film info
				filmUpdate.Image = fileNameNewImage;
			}

			//updae film info
			filmUpdate.GenreId = film.GenreId;
			filmUpdate.CountryCode = film.CountryCode;
			filmUpdate.Title = film.Title;
			filmUpdate.Description = film.Description;
			filmUpdate.FilmDuration = film.FilmDuration;
			cinemaContext.Film.Update(filmUpdate);
			cinemaContext.SaveChanges();
		}

		public bool IsFilmTitleExisted(string title)
		{
			CinemaContext cinemaContext = new CinemaContext();
			return cinemaContext.Film.FirstOrDefault(x => x.Title == title) != null;
		}

		public Film? GetFilmById(long id)
		{
			return new CinemaContext().Film.Include(x => x.Genre).Include(x => x.Shows).FirstOrDefault(x => x.FilmId == id);
		}

		public long CountFilm() => GetAll().Count();

		public List<Film> GetAll() => new CinemaContext().Film.Include(x => x.Genre).Include(x => x.Shows).ToList();

		public List<Film> GetFilmHaveShow()
		{
			List<Film> films = GetAll().Where(x => x.Shows.Any(s => DateTime.Compare(s.ShowDate, DateTime.Now) > 0))
				.Select(x => new Film
				{
					Country = x.Country,
					CountryCode = x.CountryCode,
					DateRelease = x.DateRelease,
					Description = x.Description,
					FilmDuration = x.FilmDuration,
					FilmId = x.FilmId,
					Genre = x.Genre,
					GenreId = x.GenreId,
					Image = x.Image,
					Title = x.Title,
					Shows = x.Shows.Where(s => DateTime.Compare(s.ShowDate, DateTime.Now) > 0).ToList()
				}).ToList();
			return films.Where(x => x.Shows.Count > 0).ToList();
		}

		public List<Film> GetFilmNoShow()
		{
			List<Film> films = GetAll().Where(x => x.Shows.All(s => DateTime.Compare(s.ShowDate, DateTime.Now) <= 0)).ToList();
			return films;
		}
	}
}
