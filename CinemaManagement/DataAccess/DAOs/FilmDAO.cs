﻿using BusinessObject;
using DataAccess.Utilities;
using Microsoft.AspNetCore.Http;
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

        public void CreateFilm(Film film, IFormFile? image)
        {
            if (IsFilmTitleExisted(film.Title)) throw new Exception("Film's title was existed!");
            Util util = new Util();
            string pathFolder = "\\Data\\Images";
            string pathSaveImage = Directory.GetCurrentDirectory() + pathFolder;
            string fileName = Guid.NewGuid().ToString() + image.FileName.Substring(image.FileName.LastIndexOf("."));
            util.SaveFile(image, pathSaveImage, fileName);
            film.DateRelease = DateTime.Now;
            film.Image = fileName;
            CinemaContext.Instance.Film.Add(film);
            CinemaContext.Instance.SaveChanges();
        }

        public bool IsFilmTitleExisted(string title)
        {
            return CinemaContext.Instance.Film.FirstOrDefault(x => x.Title == title) != null;
        }
    }
}
