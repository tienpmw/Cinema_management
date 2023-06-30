using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
	public class RoomDAO
	{
        //Using Singleton Pattern
        private static RoomDAO instance = null;
        private static readonly object instanceLock = new object();

        public static RoomDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new RoomDAO();
                    }
                }
                return instance;
            }
        }

        public void CreateRoom(Room room)
        {
            CinemaContext cinemaContext = new CinemaContext();
            if (RoomNameExisted(room.RoomName)) throw new Exception("Room's name was existed!");
            cinemaContext.Add(room);
            cinemaContext.SaveChanges();   
        }

        private bool RoomNameExisted(string roomName) 
        {
            CinemaContext cinemaContext = new CinemaContext();
            return cinemaContext.Room.FirstOrDefault(x => x.RoomName == roomName) != null; 
        }

        
    }
}
