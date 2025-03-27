using Microsoft.Extensions.Options;
using UserService.Infrastructure.Interfaces.Services;
using UserService.Infrastructure.Options;

namespace UserService.Infrastructure.Services
{
    public class ConfirmCodeService : IConfirmCodeService
    {
        private readonly ICacheService _cacheService;
        private readonly ConfirmCodeOptions _confirmCodeOptions;
        private readonly Random _random;

        public ConfirmCodeService(
            ICacheService cacheService,
            IOptions<ConfirmCodeOptions> options)
        {
            _cacheService = cacheService;
            _confirmCodeOptions = options.Value;
            _random = new Random();
        }

        public async Task<string> GenerateCodeAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var code = GenerateNumericCode(_confirmCodeOptions.CodeLength);

            await _cacheService.SetAsync(
                $"confirm_code_{code}",
                userId,
                cancellationToken,
                TimeSpan.FromMinutes(_confirmCodeOptions.ExpiresMinutes));

            return code;
        }

        public async Task<Guid> ConfirmCodeAsync(
            string code,
            CancellationToken cancellationToken)
        {
            var storedUserId= await _cacheService.GetAsync<string>(
                $"confirm_code_{code}",
                cancellationToken);

            if(string.IsNullOrEmpty(storedUserId))
                return Guid.Empty;

            await _cacheService.DeleteAsync(
                $"confirm_code_{code}",
                cancellationToken);

            return Guid.Parse(storedUserId);
        }

        private string GenerateNumericCode(int length)
        {
            var min = (int)Math.Pow(10, length - 1);
            var max = (int)Math.Pow(10, length) - 1;

            return (_random.Next(min, max + 1) * 1000L + DateTime.UtcNow.Millisecond) % (max - min + 1) + min + "";
        }
    }
}
