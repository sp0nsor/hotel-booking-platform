using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using UserService.Application.Interfaces;
using UserService.Application.Options;
using UserService.Infrastructure.Data.Entities;
using UserService.Infrastructure.Data.Specifications;
using UserService.Infrastructure.Interfaces.Data;

namespace UserService.Application.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly RefreshTokenOptions _options;
        private readonly IRepository<RefreshTokenEntity> _repository;

        public RefreshTokenService(
            IOptions<RefreshTokenOptions> options,
            IRepository<RefreshTokenEntity> repository)
        {
            _options = options.Value;
            _repository = repository;
        }

        public async Task<string> CreateResreshTokenAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var value = GenerateValue();

            var entity = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                Value = value,
                UserId = userId,
                Expires = DateTime.UtcNow.AddDays(_options.ExpiresDays)
            };

            await _repository.CreateAsync(
                entity,
                cancellationToken);

            return entity.Value;
        }

        public async Task DeleteTokenAsync(
            string refreshTokenValue, 
            CancellationToken cancellationToken)
        {
            var specification = new GetRefreshTokenByValueSpecification(refreshTokenValue);

            var tokenEntity = await _repository.GetSingleAsync(
                specification,
                cancellationToken);

            if (tokenEntity is null)
                return;

            await _repository.DeleteAsync(
                tokenEntity, 
                cancellationToken);
        }

        public async Task<RefreshTokenEntity> GetTokenByValueAsync(
            string value,
            CancellationToken cancellationToken)
        {
            var specification = new GetRefreshTokenByValueSpecification(value);

            var tokenEntity = await _repository.GetSingleAsync(
                specification,
                cancellationToken);

            return tokenEntity;
        }

        private string GenerateValue()
        {
            var randomBytes = new byte[_options.TokenLength];

            RandomNumberGenerator.Fill(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}
