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
            if (RoomNameExisted(room.RoomName)) throw new Exception("Room's name was existed!");
            CinemaContext.Instance.Add(room);
            CinemaContext.Instance.SaveChanges();   
        }

        private bool RoomNameExisted(string roomName) 
        {
            return CinemaContext.Instance.Room.FirstOrDefault(x => x.RoomName == roomName) != null; 
        }
    }
}
