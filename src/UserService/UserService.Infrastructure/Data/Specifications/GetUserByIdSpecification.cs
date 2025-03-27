using System.Linq.Expressions;
using UserService.Infrastructure.Data.Entities;

namespace UserService.Infrastructure.Data.Specifications
{
    public class GetUserByIdSpecification : Specification<UserEntity>
    {
        private readonly Guid _id;

        public GetUserByIdSpecification(Guid id)
        {
            _id = id;

            AddInclude(user => user.Role);
        }

        public override Expression<Func<UserEntity, bool>> ToExpression()
        {
            return user => user.Id == _id;
        }
    }
}
