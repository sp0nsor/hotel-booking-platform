using UserService.Infrastructure.Data.Entities;

namespace UserService.Application.Interfaces
{
    public interface IAccessTokenService
    {
        Task<string> CreateAccessTokenAsync(UserEntity user, CancellationToken cancellationToken);
        Task DeleteTokenAsync(string jwtTokenId, CancellationToken cancellationToken);
    }
}