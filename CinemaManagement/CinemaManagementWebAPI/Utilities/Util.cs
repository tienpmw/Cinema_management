using BusinessObject;
using DataAccess.DAOs;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Text.Json;

namespace CinemaWebAPI.Utilities
{
    public class Util
    {
        //Using Singleton Pattern
        private static Util instance = null;
        private static readonly object instanceLock = new object();
        public static Util Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new Util();
                    }
                }
                return instance;
            }
        }


        public string GetRandomString(int length)
        {
            StringBuilder str_build = new StringBuilder();
            Random random = new Random();

            char letter;

            for (int i = 0; i < length; i++)
            {
                double flt = random.NextDouble();
                int shift = Convert.ToInt32(Math.Floor(25 * flt));
                letter = Convert.ToChar(shift + 65);
                str_build.Append(letter);
            }
            return str_build.ToString();
        }

        public void WriteFile(string fileName, object data)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(fullPath, json);
        }

        public string ReadFile(string fileName)
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            string text = File.ReadAllText(fullPath);
            return text;
        }
    }
}
