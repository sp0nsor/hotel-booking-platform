using BookingService.Application.Interfaces;
using BookingService.Application.Requests;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("users/{userId}/bookings")]
        public async Task<ActionResult> GetBookingsByUserId(
            [FromRoute] Guid userId, 
            [FromQuery] GetBookingsRequest getBookingsRequest, 
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.GetBookingsByUserIdAsync(
                userId,
                getBookingsRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpPost("hotels/{hotelId}/rooms/{roomId}/bookings")]
        public async Task<ActionResult> CreateBooking(
            [FromRoute] Guid hotelId,
            [FromRoute] Guid roomId,
            [FromBody] CreateBookingRequest bookingRequest, 
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.CreateBookingAsync(
                hotelId,
                roomId,
                bookingRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok() 
                : BadRequest(result.Error);
        }

        [HttpPut("users/{userId}/bookings/{bookingId}")]
        public async Task<ActionResult> UpdateBooking(
            [FromRoute] Guid userId,
            [FromRoute] Guid bookingId,
            [FromBody] CreateBookingRequest bookingRequest,
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.UpdateBookingAsync(
                userId,
                bookingId,
                bookingRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok() 
                : BadRequest(result.Error);
        }

        [HttpPatch("users/{userId}/bookings/{bookingId}/cancelled")]
        public async Task<ActionResult> CancelBooking(
            [FromRoute] Guid userId,
            [FromRoute] Guid bookingId,
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.CancelBookingAsync(
                userId,
                bookingId,
                cancellationToken);

            return result.IsSuccess 
                ? Ok() 
                : BadRequest(result.Error);
        }
    }
}
