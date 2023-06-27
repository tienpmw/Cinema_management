using DataAccess.DAOs;
using System.Text;

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
    }
}
