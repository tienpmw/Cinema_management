﻿using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class GenreDAO
    {
        //Using Singleton Pattern
        private static GenreDAO instance = null;
        private static readonly object instanceLock = new object();

        public static GenreDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new GenreDAO();
                    }
                }
                return instance;
            }
        }

        public List<Genre> GetAll()
        {
            return new CinemaContext().Genre.ToList();
        }
    }
}
