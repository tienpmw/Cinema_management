﻿using AutoMapper;
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
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<Film, FilmDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Show, ShowDTO>().ReverseMap();
            CreateMap<Booking, BookingDTO>().ReverseMap();
        }
    }
}
