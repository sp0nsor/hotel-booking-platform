using BookingService.Application.DTOs;
using BookingService.Application.Requests;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Interfaces.Public
{
    public interface IBookingService
    {
        Task<Result<PaginatedResult<BookingDto>>> GetBookingsByUserIdAsync(Guid userId, GetBookingsRequest getBookingsRequest, CancellationToken cancellationToken);
        Task<Result<PaginatedResult<BookingDto>>> GetBookingsByHotelIdAsync(Guid hotelId, GetBookingsRequest getBookingsRequest, CancellationToken cancellationToken);
        Task<Result> CreateBookingAsync(Guid userId, Guid hotelId, Guid roomId, CreateBookingRequest bookingRequest, CancellationToken cancellationToken);
        Task<Result> UpdateBookingAsync(Guid userId, Guid bookingId, CreateBookingRequest bookingRequest, CancellationToken cancellationToken);
        Task<Result> CancelBookingAsync(Guid userId, Guid bookingId, CancellationToken cancellationToken);
    }
}