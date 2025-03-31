using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Infrastructure.Options;
using UserService.Infrastructure.Data.Entities;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Application.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ITokenService _tokenService;
        private readonly AccessTokenOptions _options;
        private readonly ICacheService _cacheService;

        public AccessTokenService(
            ICacheService cacheService,
            IOptions<AccessTokenOptions> options,
            ITokenService tokenService)
        {
            _cacheService = cacheService;
            _options = options.Value;
            _tokenService = tokenService;
        }

        public async Task<string> CreateAccessTokenAsync(
            UserEntity user,
            CancellationToken cancellationToken)
        {
            var (jwtTokenId, tokenValue) = _tokenService.GenerateAccessToken(user);

            await _cacheService.SetAsync(
                $"access_{jwtTokenId}",
                tokenValue,
                cancellationToken,
                TimeSpan.FromMinutes(_options.ExpiresMinutes));

            return tokenValue;
        }

        public async Task DeleteTokenAsync(
            string jwtTokenId,
            CancellationToken cancellationToken)
        {
            await _cacheService.DeleteAsync(
                $"access_{jwtTokenId}",
                cancellationToken);
        }
    }
}
