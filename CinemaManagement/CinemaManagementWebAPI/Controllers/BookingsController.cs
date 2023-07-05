using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CinemaWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class BookingsController : ControllerBase
	{
		private readonly IShowRepository _showRepository;
		private readonly IBookingRepository _bookRepository;
		private readonly IUserRepository _userRepository;

		public BookingsController(IShowRepository showRepository, IBookingRepository bookRepository, IUserRepository userRepository)
		{
			_showRepository = showRepository;
			_bookRepository = bookRepository;
			_userRepository = userRepository;
		}

		
		[HttpGet("{idShow}/{idUser}")]
		public IActionResult GetBookingByUserId(long idShow, long idUser)
		{
			return Ok(_bookRepository.FindBookingByUserId(idShow, idUser));
		}


		[Authorize]
		[HttpPost("{id}")]
		public IActionResult AddBooking(long id, BookingRequestDTO booking)
		{
			Show? show = _showRepository.GetShowById(id);
			User? user = _userRepository.GetUserById(booking.UserId);
			if (user == null) return NotFound("User not found!");
			if (show == null) return NotFound("Show not found!");
			if (DateTime.Compare(show.ShowDate, DateTime.Now) < 0) return Conflict();
			List<char> list = show.SeatStatus.Where((v, i) => booking.SeatsBooking.Contains(i.ToString()) && v == '1').ToList();
			if (list.Count > 0)
			{
				return Conflict("Seat has been booked!");
			}
			if (show.Price * booking.SeatsBooking.Length > user.AccountBalance)
			{
				return BadRequest("Balance not enough to booking seat!");
			}
			char[] seatsBooking = new string('0', show.Room.NumberRow * show.Room.NumberColumn).ToCharArray();
			char[] seatsCurrentShow = show.SeatStatus.ToCharArray();
			foreach (string item in booking.SeatsBooking)
			{
				int index = int.Parse(item);
				seatsBooking[index] = '1';
				seatsCurrentShow[index] = '1';
			}
			Booking b = new Booking
			{
				BookingId = Guid.NewGuid(),
				UserId = user.UserId,
				Amount = show.Price * booking.SeatsBooking.Length,
				ContentBill = "Booking seats show #" + show.ShowId,
				DateBooking = DateTime.Now,
				IsPay = true,
				ShowId = show.ShowId,
				SeatBooking = new string(seatsBooking),
			};
			show.SeatStatus = new string(seatsCurrentShow);
			user.AccountBalance -= show.Price * booking.SeatsBooking.Length;
			try
			{
				_bookRepository.AddBooking(show, b, user);
			}
			catch (Exception)
			{
				return Conflict("Something wrong, try later!");
			}
			return Ok();
		}
	}
}
