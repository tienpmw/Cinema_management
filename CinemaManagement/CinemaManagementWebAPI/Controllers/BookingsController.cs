﻿using BusinessObject;
using DataAccess.IRepositories;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace CinemaWebAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BookingsController : ControllerBase
	{
		private readonly IShowRepository _showRepository;
		private readonly IBookingRepository _bookRepository;
		private readonly IUserRepository _userRepository;
		private readonly ISendMailRepository _sendRepository;

		public BookingsController(IShowRepository showRepository, IBookingRepository bookRepository, 
			IUserRepository userRepository, ISendMailRepository sendRepository)
		{
			_showRepository = showRepository;
			_bookRepository = bookRepository;
			_userRepository = userRepository;
			_sendRepository = sendRepository;
		}


		[HttpGet]
		[EnableQuery]
		public IActionResult GetShows()
		{
			return Ok(new CinemaContext().Booking.AsQueryable());
		}


		[HttpGet("{idShow}/{idUser}")]
		public IActionResult GetBookingByUserId(long idShow, long idUser)
		{
			return Ok(_bookRepository.FindBookingByUserId(idShow, idUser));
		}


		[Authorize]
		[HttpPost("{id}")]
		public async Task<IActionResult> AddBooking(long id, BookingRequestDTO booking)
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
				string message = $"<div>\r\n      " +
					$"<h4>Hello, {user.Email}!</h4>\r\n      " +
					$"<p>Information your booking show film:</p>\r\n      " +
					$"<table>\r\n        " +
					$"<tr>\r\n          " +
					$"<td>Code Booking</td>\r\n          " +
					$"<td>{b.BookingId}</td>\r\n       " +
					$" </tr>\r\n        " +
					$"<tr>\r\n          " +
					$"<td>Film Name</td>\r\n          " +
					$"<td>{show.Film.Title}</td>\r\n        " +
					$"</tr>\r\n        <tr>\r\n          " +
					$"<td>Time</td>\r\n          " +
					$"<td>{show.ShowDate.ToString("dd/MM/yyyy hh:mm tt")}</td>\r\n        " +
					$"</tr>\r\n        " +
					$"<tr>\r\n          " +
					$"<td>Seat booking</td>\r\n          " +
					$"<td>{string.Join(", ", booking.SeatsBooking.OrderBy(x => x))}</td>\r\n        " +
					$"</tr>\r\n        " +
					$"<tr>\r\n          " +
					$"<td>Total Amount</td>\r\n          " +
					$"<td>{b.Amount.ToString("C0").Substring(1)} VNĐ</td>\r\n        " +
					$"</tr>\r\n      " +
					$"</table>\r\n    " +
					$"</div>";
				await _sendRepository.SendEmailAsync(user.Email, $"Booking seat for film {show.Film.Title} successful.", message);
			}
			catch (Exception)
			{
				return Conflict("Something wrong, try later!");
			}
			return Ok();
		}
	}
}
