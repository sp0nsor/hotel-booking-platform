using BookingService.Infrastructure.Data.Entities;
using System.Linq.Expressions;

namespace BookingService.Infrastructure.Data.Specifications
{
    public class HotelBookingsSpecification : Specification<BookingEntity>
    {
        private readonly Guid _hotelId;
        private readonly bool? _isOutdated;
        private readonly string? _searchFirstName;
        private readonly string? _searchLastName;

        public HotelBookingsSpecification(
            Guid hotelId,
            bool? isOutdated,
            string? searchFirstName,
            string? searchLastName)
        {
            _hotelId = hotelId;
            _isOutdated = isOutdated;
            _searchFirstName = searchFirstName;
            _searchLastName = searchLastName;
        }

        public override Expression<Func<BookingEntity, bool>> ToExpression()
        {
            return booking =>
                booking.HotelId == _hotelId &&
                (_isOutdated == null || booking.IsOutdated == _isOutdated) &&
                (string.IsNullOrWhiteSpace(_searchFirstName) || booking.GuestFirstName.ToLower().Contains(_searchFirstName.ToLower())) &&
                (string.IsNullOrWhiteSpace(_searchLastName) || booking.GuestLastName.ToLower().Contains(_searchLastName.ToLower()));
        }
    }
}
