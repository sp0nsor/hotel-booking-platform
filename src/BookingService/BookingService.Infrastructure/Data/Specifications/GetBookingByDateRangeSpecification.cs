using BookingService.Infrastructure.Data.Entities;
using System.Linq.Expressions;

namespace BookingService.Infrastructure.Data.Specifications
{
    public class GetBookingByDateRangeSpecification
        : Specification<BookingEntity>
    {
        private readonly Guid _hotelId;
        private readonly Guid _roomId;
        private readonly DateTime _startDate;
        private readonly DateTime _endDate;
        private readonly Guid? _excludeBookingId;

        public GetBookingByDateRangeSpecification(
            Guid hotelId,
            Guid roomId,
            DateTime startDate,
            DateTime endDate,
            Guid? excludeBookingId = null)
        {
            _hotelId = hotelId;
            _roomId = roomId;
            _startDate = startDate;
            _endDate = endDate;
            _excludeBookingId = excludeBookingId;
        }

        public override Expression<Func<BookingEntity, bool>> ToExpression()
        {
            return booking =>
                booking.HotelId == _hotelId && 
                booking.RoomId == _roomId && 
                (booking.StartDate <= _endDate && booking.EndDate >= _startDate) &&
                (_excludeBookingId == null || booking.Id != _excludeBookingId);
        }
    }
}
