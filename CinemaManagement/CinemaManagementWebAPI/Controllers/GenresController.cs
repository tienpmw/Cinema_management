﻿using AutoMapper;
using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Attributes;

namespace CinemaWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenresController(IGenreRepository genreRepository, IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        [EnableQuery]
        [ODataAttributeRouting]
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_mapper.Map<List<GenreDTO>>(_genreRepository.GetAll()));
        }
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public IActionResult AddGenre([FromBody]string nameGenre)
		{
            if(string.IsNullOrEmpty(nameGenre.Trim()))
            {
                return Conflict();
            }
            _genreRepository.AddGenre(nameGenre);
            return Ok();
		}
		[Authorize(Roles = "Admin")]
		[HttpPut("{id}")]
		public IActionResult EditGenre(long id, [FromBody] Genre genre)
		{
            if(id != genre.GenreId)
            {
				return Conflict();
			}
			if (string.IsNullOrEmpty(genre.GenreName.Trim()))
			{
				return Conflict();
			}
			_genreRepository.EditGenre(genre);
			return Ok();
		}
	}
}
