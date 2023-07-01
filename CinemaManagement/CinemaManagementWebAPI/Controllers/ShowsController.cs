using AutoMapper;
using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ShowsController : ControllerBase
	{
		private readonly IShowRepository _showRepository;
		private readonly IMapper _mapper;

		public ShowsController(IShowRepository showRepository, IMapper mapper)
		{
			_showRepository = showRepository;
			_mapper = mapper;
		}
		[HttpGet]
		public IActionResult GetShows()
		{
			return Ok(_mapper.Map<List<ShowDTO>>(_showRepository.GetShows()));
		}
		[HttpPost]
		public IActionResult AddShow(ShowDTO show)
		{
			Show newShow = new Show
			{
				ShowDate = show.ShowDate,
				FilmId = show.ShowId,
				RoomId = show.RoomId,
				Price = show.Price,
			};
			try
			{
				_showRepository.AddShow(newShow);
			}
			catch (Exception)
			{

				return Conflict();
			}
			return Ok();
		}
	}
}
