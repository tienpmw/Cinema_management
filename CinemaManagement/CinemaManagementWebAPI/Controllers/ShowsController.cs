using AutoMapper;
using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

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
		[EnableQuery]
		public IActionResult GetShows()
		{
			return Ok(_mapper.Map<List<ShowDTO>>(_showRepository.GetShows()));
		}
		[HttpGet("{id}")]
		public IActionResult GetShowById(long id)
		{
			Show? show = _showRepository.GetShowById(id);
			if (show == null)
			{
				return NotFound();
			}
			return Ok(_mapper.Map<ShowDTO>(show));
		}
		[HttpGet("GetEarningMonthly")]
		public IActionResult GetEarningMonthly()
		{

			long earning = _showRepository.GetEarningMonthly();
			return Ok(new
			{
				earning = earning
			});
		}
		[HttpGet("GetEarningAnnual")]
		public IActionResult GetEarningAnnual()
		{

			List<dynamic> earning = _showRepository.GetEarningAnnual();
			return Ok(new
			{
				earning = earning
			});
		}
		[HttpPost]
		public IActionResult AddShow(ShowDTO show)
		{
			Show newShow = new Show
			{
				ShowDate = show.ShowDate,
				FilmId = show.FilmId,
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
		[HttpPut]
		public IActionResult UpdateShow(ShowDTO show)
		{
			Show? show1 = _showRepository.GetShowById(show.ShowId);
			if (show == null)
			{
				return NotFound();
			}
			if (show1.ShowDate.Subtract(DateTime.Now) <= new TimeSpan(1,0,0))
			{
				return Conflict();
			}
			Show newShow = _mapper.Map<Show>(show);
			try
			{
				_showRepository.UpdateShow(newShow);
			}
			catch (Exception)
			{

				return Conflict();
			}
			return Ok();
		}
		[HttpDelete("{id}")]
		public IActionResult DeleteShow(long id)
		{
			Show? show = _showRepository.GetShowById(id);
			if (show == null)
			{
				return NotFound();
			}
			if (show.ShowDate.Subtract(DateTime.Now) <= new TimeSpan(1, 0, 0))
			{
				return Conflict();
			}
			try
			{
				_showRepository.DeleteShow(show);
			}
			catch (Exception)
			{

				return Conflict();
			}
			return Ok();
		}
	}
}
