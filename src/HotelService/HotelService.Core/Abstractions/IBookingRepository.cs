using HotelService.Core.Models;

namespace HotelService.Core.Abstractions
{
    public interface IBookingRepository : IRepository<BookingPeriod>
    {
        Task<BookingPeriod?> GetByIdAsync(Guid bookingId, Guid roomId, CancellationToken cancellationToken);
    }
}