using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Application.Interfaces;
using UserService.Application.Options;
using UserService.Infrastructure.Data.Entities;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Application.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        public readonly AccessTokenOptions _options;
        public readonly ICacheService _cacheService;

        public AccessTokenService(
            ICacheService cacheService,
            IOptions<AccessTokenOptions> options)
        {
            _cacheService = cacheService;
            _options = options.Value;
        }

        public async Task<string> CreateAccessTokenAsync(
            UserEntity user,
            CancellationToken cancellationToken)
        {
            var jwtTokenId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, jwtTokenId),
                new(ClaimTypes.Role, user.Role.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                audience: _options.Audience,
                issuer: _options.Issuer,
                expires: DateTime.UtcNow.AddMinutes(
                    _options.ExpiresMinutes));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

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
