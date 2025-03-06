using BookingService.Application.Interfaces;
using BookingService.Application.Requests;
using Microsoft.AspNetCore.Mvc;

namespace BookingService.API.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingsService;

        public BookingsController(IBookingService bookingsService)
        {
            _bookingsService = bookingsService;
        }

        [HttpGet("{hotelId}")]
        public async Task<ActionResult> GetHotelBookings(
            [FromRoute] Guid hotelId,
            [FromQuery] GetBookingsRequest getBookingsRequest,
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.GetHotelBookingsAsync(
                hotelId,
                getBookingsRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok(result.Value)
                : BadRequest(result.Error);
        }

        [HttpPost]
        public async Task<ActionResult> CreateBooking(
            [FromBody] BookingDataRequest bookingDataRequest, 
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.CreateBookingAsync(
                bookingDataRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok() 
                : BadRequest(result.Error);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateBooking(
            [FromRoute] Guid id,
            [FromBody] BookingDataRequest bookingDataRequest,
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.UpdateBookingAsync(
                id,
                bookingDataRequest,
                cancellationToken);

            return result.IsSuccess
                ? Ok() 
                : BadRequest(result.Error);
        }

        [HttpPatch("{id}/cancel")]
        public async Task<ActionResult> CancelBooking(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.CancelBookingAsync(
                id,
                cancellationToken);

            return result.IsSuccess 
                ? Ok() 
                : BadRequest(result.Error);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBooking(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _bookingsService.DeleteBookingAsync(
                id, 
                cancellationToken);

            return result.IsSuccess 
                ? Ok() 
                : BadRequest(result.Error);
        }
    }
}
