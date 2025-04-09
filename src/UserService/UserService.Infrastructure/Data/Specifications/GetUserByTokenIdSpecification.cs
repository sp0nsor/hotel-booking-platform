using System.Linq.Expressions;
using UserService.Infrastructure.Data.Entities;

namespace UserService.Infrastructure.Data.Specifications
{
    public class GetUserByTokenIdSpecification : Specification<UserEntity>
    {
        private readonly Guid _tokenId;

        public GetUserByTokenIdSpecification(Guid tokenId)
        {
            _tokenId = tokenId;

            AddInclude(user => user.Role);
        }

        public override Expression<Func<UserEntity, bool>> ToExpression()
        {
            return user => user.RefreshTokenId == _tokenId;
        }
    }
}
