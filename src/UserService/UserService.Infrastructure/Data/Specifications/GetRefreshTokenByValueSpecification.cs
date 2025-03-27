using System.Linq.Expressions;
using UserService.Infrastructure.Data.Entities;

namespace UserService.Infrastructure.Data.Specifications
{
    public class GetRefreshTokenByValueSpecification : Specification<RefreshTokenEntity>
    {
        private readonly string _value;

        public GetRefreshTokenByValueSpecification(string value)
        {
            _value = value;
        }

        public override Expression<Func<RefreshTokenEntity, bool>> ToExpression()
        {
            return token => token.Value == _value;
        }
    }
}
