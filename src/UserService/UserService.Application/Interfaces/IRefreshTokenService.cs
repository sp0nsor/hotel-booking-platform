using UserService.Infrastructure.Data.Entities;

namespace UserService.Application.Interfaces
{
    public interface IRefreshTokenService
    {
        Task<string> CreateResreshTokenAsync(Guid userId, CancellationToken cancellationToken);
        Task DeleteTokenAsync(string refreshTokenValue, CancellationToken cancellationToken);
        Task<RefreshTokenEntity> GetTokenByValueAsync(string value, CancellationToken cancellationToken);
    }
}