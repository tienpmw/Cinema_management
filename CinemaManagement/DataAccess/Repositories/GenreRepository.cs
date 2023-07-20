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
    public class GenreRepository : IGenreRepository
    {
        public void AddGenre(string nameGenre) => GenreDAO.Instance.AddGenre(nameGenre);

        public void EditGenre(Genre genre) => GenreDAO.Instance.EditGenre(genre);

		public List<Genre> GetAll() => GenreDAO.Instance.GetAll();
    }
}
