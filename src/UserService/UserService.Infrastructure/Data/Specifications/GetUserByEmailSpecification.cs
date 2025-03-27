using System.Linq.Expressions;
using UserService.Infrastructure.Data.Entities;

namespace UserService.Infrastructure.Data.Specifications
{
    public class GetUserByEmailSpecification : Specification<UserEntity>
    {
        public string _email = string.Empty;

        public GetUserByEmailSpecification(string email)
        {
            _email = email;

            AddInclude(user => user.Role);
            AddInclude(user => user.RefreshToken);
        }

        public override Expression<Func<UserEntity, bool>> ToExpression()
        {
            return user => user.Email == _email;
        }
    }
}
