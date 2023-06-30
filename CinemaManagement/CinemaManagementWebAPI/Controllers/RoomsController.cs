using AutoMapper;
using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomsController(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataAttributeRouting]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok( new CinemaContext().Room.AsQueryable());
        }

        [HttpPost]
        public IActionResult Post(RoomDTO roomDTO) 
        {
            try
            {
                var room = _mapper.Map<Room>(roomDTO);
                _roomRepository.CreateRoom(room);
                return Ok("Create new room has been success!");
            }
            catch (Exception ex) 
            {
                return Conflict(ex.Message);
            }
        }
    }
}
