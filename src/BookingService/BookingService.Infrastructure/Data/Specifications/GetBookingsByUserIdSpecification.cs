using BookingService.Infrastructure.Data.Entities;
using System.Linq.Expressions;

namespace BookingService.Infrastructure.Data.Specifications
{
    public class GetBookingsByUserIdSpecification
        : Specification<BookingEntity>
    {
        private readonly Guid _userId;
        private readonly bool? _isOutdated;

        public GetBookingsByUserIdSpecification(
            Guid userId,
            bool? isOutdated)
        {
            _userId = userId;
            _isOutdated = isOutdated;
        }

        public override Expression<Func<BookingEntity, bool>> ToExpression()
        {
            return booking =>
            booking.UserId == _userId &&
            (_isOutdated == null || booking.IsOutdated == _isOutdated);
        }
    }
}
