using BookingService.Application.Interfaces;
using BookingService.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingService.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingsService;

        public BookingsController(IBookingService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("hotels/{hotelId}/bookings")]
        public async Task<ActionResult> GetBookingsByHotelId(
            [FromRoute] Guid hotelId,
            [FromQuery] GetBookingsRequest getBookingsRequest,
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.GetBookingsByHotelIdAsync(
                hotelId,
                getBookingsRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpGet("users/me/bookings")]
        public async Task<ActionResult> GetBookingsByUserId(
            [FromQuery] GetBookingsRequest getBookingsRequest, 
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _bookingsService.GetBookingsByUserIdAsync(
                Guid.Parse(userId),
                getBookingsRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpPost("hotels/{hotelId}/rooms/{roomId}/bookings")]
        public async Task<ActionResult> CreateBooking(
            [FromRoute] Guid hotelId,
            [FromRoute] Guid roomId,
            [FromBody] CreateBookingRequest bookingRequest, 
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _bookingsService.CreateBookingAsync(
                Guid.Parse(userId),
                hotelId,
                roomId,
                bookingRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok() 
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpPut("users/me/bookings/{bookingId}")]
        public async Task<ActionResult> UpdateBooking(
            [FromRoute] Guid bookingId,
            [FromBody] CreateBookingRequest bookingRequest,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _bookingsService.UpdateBookingAsync(
                Guid.Parse(userId),
                bookingId,
                bookingRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok() 
                : BadRequest(result.Error);
        }

        [Authorize]
        [HttpPatch("users/me/bookings/{bookingId}/cancelled")]
        public async Task<ActionResult> CancelBooking(
            [FromRoute] Guid bookingId,
            CancellationToken cancellationToken)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = await _bookingsService.CancelBookingAsync(
                Guid.Parse(userId),
                bookingId,
                cancellationToken);

            return result.IsSuccess 
                ? Ok() 
                : BadRequest(result.Error);
        }
    }
}
