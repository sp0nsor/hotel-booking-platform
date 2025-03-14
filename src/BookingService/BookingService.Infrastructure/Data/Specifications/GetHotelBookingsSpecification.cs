using BookingService.Infrastructure.Data.Entities;
using System.Linq.Expressions;

namespace BookingService.Infrastructure.Data.Specifications
{
    public class GetHotelBookingsSpecification
        : Specification<BookingEntity>
    {
        private readonly Guid _hotelId;
        private readonly bool? _isOutdated;

        public GetHotelBookingsSpecification(
            Guid hotelId,
            bool? isOutdated)
        {
            _hotelId = hotelId;
            _isOutdated = isOutdated;
        }

        public override Expression<Func<BookingEntity, bool>> ToExpression()
        {
            return booking =>
                booking.HotelId == _hotelId &&
                (_isOutdated == null || booking.IsOutdated == _isOutdated);
        }
    }
}
