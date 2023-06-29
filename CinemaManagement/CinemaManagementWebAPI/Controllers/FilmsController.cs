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
    public class FilmsController : ControllerBase
    {
        private readonly IFilmRepository filmRepository;
        private readonly IMapper mapper;

        public FilmsController(IFilmRepository filmRepository, IMapper mapper)
        {
            this.filmRepository = filmRepository;
            this.mapper = mapper;
        }

        [EnableQuery]
        [ODataAttributeRouting]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(CinemaContext.Instance.Film.AsQueryable());
        }

        [HttpPost]
        public IActionResult Post([FromForm] IFormFile imageFile, [FromForm] FilmDTO filmDTO) 
        {
            try
            {
                var film = mapper.Map<FilmDTO>(filmDTO);    
                return Ok();
            }catch (Exception ex) 
            {
                return Conflict(ex);
            }
        }
    }

}
