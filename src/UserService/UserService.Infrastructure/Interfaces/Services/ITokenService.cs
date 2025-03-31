using UserService.Infrastructure.Data.Entities;

namespace UserService.Infrastructure.Interfaces.Services
{
    public interface ITokenService
    {
        (string TokenId, string tokenValue) GenerateAccessToken(UserEntity user);
        string GenerateRefreshToken();
    }
}