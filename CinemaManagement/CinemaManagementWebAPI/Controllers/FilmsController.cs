using AutoMapper;
using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using System.Text.Json;

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
            return Ok(new CinemaContext().Film.AsQueryable());
        }


        [HttpGet("GetFilmById/{id}")]
        public IActionResult Get(int id)
        {
            var result = new CinemaContext().Film.FirstOrDefault(x => x.FilmId == id);
            if (result == null) return NotFound();
            FilmDTO filmDTO = mapper.Map<FilmDTO>(result);  
            return Ok(filmDTO);
        }


        [HttpPost]
        public IActionResult Post() 
        {

            try
            {
                var imageFile = Request.Form.Files["imageFile"];
                var filmDTOJson = Request.Form["filmDTO"];
                var filmDTO = JsonSerializer.Deserialize<FilmDTO>(filmDTOJson);
                var film = mapper.Map<Film>(filmDTO);

                filmRepository.CreateFilm(film, imageFile);
                return Ok("Create new Film has been success!");
            }catch (Exception ex) 
            {
                return Conflict(ex.Message);
            }
        }


        [HttpPut]
        public IActionResult Put(FilmDTO filmDTO) 
        {
            try
            {

            }catch(Exception ex) 
            {

            }
            return Ok();
        }
    }

}
