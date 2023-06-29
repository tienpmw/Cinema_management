using AutoMapper;
using BusinessObject;
using DTOs;
using System.Data;

namespace CinemaWebAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Transaction, TransactionDTO>().ReverseMap();
            CreateMap<Room, RoomDTO>().ReverseMap();
<<<<<<< HEAD
            CreateMap<Film, FilmDTO>().ReverseMap();
=======
            CreateMap<Role, RoleDTO>().ReverseMap();
>>>>>>> 04391482f60e5a934538660b0b3fedf887618f7f
        }
    }
}
