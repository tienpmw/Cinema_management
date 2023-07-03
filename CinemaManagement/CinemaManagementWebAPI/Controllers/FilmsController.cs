using AutoMapper;
using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CinemaWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FilmsController : ControllerBase
	{
		private readonly IFilmRepository _filmRepository;
		private readonly IMapper _mapper;

		public FilmsController(IFilmRepository filmRepository, IMapper mapper)
		{
			_filmRepository = filmRepository;
			_mapper = mapper;
		}

		[EnableQuery]
		[ODataAttributeRouting]
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(_mapper.Map<List<FilmDTO>>(new CinemaContext().Film.Include(x => x.Genre).ToList()));
		}


		[HttpGet("GetFilmById/{id}")]
		public IActionResult Get(long id)
		{
			var result = _filmRepository.GetFilmById(id);
			if (result == null) return NotFound();
			FilmDTO filmDTO = _mapper.Map<FilmDTO>(result);
			return Ok(filmDTO);
		}

		[HttpGet("TotalFilms")]
		public IActionResult GetTotalFilm()
		{
			return Ok(_filmRepository.CountFilm());
		}


		[HttpPost]
		public IActionResult Post()
		{

			try
			{
				var imageFile = Request.Form.Files["imageFile"];
				var filmDTOJson = Request.Form["filmDTO"];
				var filmDTO = JsonSerializer.Deserialize<FilmDTO>(filmDTOJson);
				var film = _mapper.Map<Film>(filmDTO);

				_filmRepository.CreateFilm(film, imageFile);
				return Ok("Create new Film has been success!");
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}
		}


		[HttpPut]
		public IActionResult Put()
		{
			try
			{
				var imageFile = Request.Form.Files["imageFile"];
				var filmDTOJson = Request.Form["filmDTO"];
				var filmDTO = JsonSerializer.Deserialize<FilmDTO>(filmDTOJson);
				var film = _mapper.Map<Film>(filmDTO);

				_filmRepository.UpdateFilm(film, imageFile);
				return Ok("Update Film's info has been success!");
			}
			catch (Exception ex)
			{
				return Conflict(ex.Message);
			}
		}
	}

}
