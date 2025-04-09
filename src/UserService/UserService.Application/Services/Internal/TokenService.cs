using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Application.Options;
using UserService.Infrastructure.Data.Entities;
using System.Security.Cryptography;
using UserService.Application.Interfaces.Internal;

namespace UserService.Application.Services.Internal
{
    public class TokenService : ITokenService
    {
        private readonly AccessTokenOptions _accessTokenOptions;
        private readonly RefreshTokenOptions _refreshTokenOptions;

        public TokenService(
            IOptions<RefreshTokenOptions> refreshTokenOptions,
            IOptions<AccessTokenOptions> accessTokenOptions)
        {
            _refreshTokenOptions = refreshTokenOptions.Value;
            _accessTokenOptions = accessTokenOptions.Value;
        }

        public (string TokenId, string tokenValue) GenerateAccessToken(
            UserEntity user)
        {
            var jwtTokenId = Guid.NewGuid().ToString();

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Jti, jwtTokenId),
                new(ClaimTypes.Role, user.Role.Name),
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_accessTokenOptions.SecretKey)),
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: credentials,
                issuer: _accessTokenOptions.Issuer,
                expires: DateTime.UtcNow.AddMinutes(
                    _accessTokenOptions.ExpiresMinutes));

            var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

            return (jwtTokenId, tokenValue);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[_refreshTokenOptions.TokenLength];

            RandomNumberGenerator.Fill(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
