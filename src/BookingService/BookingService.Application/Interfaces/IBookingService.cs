using BookingService.Application.DTOs;
using BookingService.Application.Requests;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Interfaces
{
    public interface IBookingService
    {
        Task<Result> CancelBookingAsync(Guid id, CancellationToken cancellationToken);
        Task<Result> CreateBookingAsync(BookingDataRequest bookingDataRequest, CancellationToken cancellationToken);
        Task<Result> DeleteBookingAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<PaginatedResult<BookingDto>>> GetHotelBookingsAsync(Guid hotelId, GetBookingsRequest getBookingsRequest, CancellationToken cancellationToken);
        Task<Result> UpdateBookingAsync(Guid id, BookingDataRequest bookingDataRequest, CancellationToken cancellationToken);
    }
}