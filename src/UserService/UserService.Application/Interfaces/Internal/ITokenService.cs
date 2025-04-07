using UserService.Infrastructure.Data.Entities;

namespace UserService.Application.Interfaces.Internal
{
    public interface ITokenService
    {
        (string TokenId, string tokenValue) GenerateAccessToken(UserEntity user);
        string GenerateRefreshToken();
    }
}
