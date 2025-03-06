using System.Linq.Expressions;

namespace BookingService.Infrastructure.Data.Specifications
{
    public abstract class Specification<T>
    {
        public abstract Expression<Func<T, bool>> ToExpression();
    }
}
