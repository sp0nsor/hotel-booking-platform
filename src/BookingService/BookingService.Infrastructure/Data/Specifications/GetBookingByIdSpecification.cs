using BookingService.Infrastructure.Data.Entities;
using System.Linq.Expressions;

namespace BookingService.Infrastructure.Data.Specifications
{
    public class GetBookingByIdSpecification 
        : Specification<BookingEntity>
    {
        private readonly Guid _bookingId;

        public GetBookingByIdSpecification(Guid bookingId)
        {
            _bookingId = bookingId;
        }

        public override Expression<Func<BookingEntity, bool>> ToExpression()
        {
            return booking => booking.Id == _bookingId;
        }
    }
}
