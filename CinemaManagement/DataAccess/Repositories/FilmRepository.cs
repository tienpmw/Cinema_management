﻿using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class FilmRepository : IFilmRepository
    {
        public void CreateFilm(Film film)
        {
            FilmDAO.Instance.CreateFilm(film);  
        }
    }
}
