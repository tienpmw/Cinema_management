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
    public class RoomRepository : IRoomRepository
    {
        public void CreateRoom(Room room)
        {
            RoomDAO.Instance.CreateRoom(room);
        }
    }
}
