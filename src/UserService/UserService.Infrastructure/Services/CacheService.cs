using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;

        public CacheService(
            IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T?> GetAsync<T>(
            string key,
            CancellationToken cancellationToken)
        {
            var value = await _cache.GetStringAsync(key, cancellationToken);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task SetAsync<T>(
            string key,
            T value,
            CancellationToken cancellationToken,
            TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };

            var serializedValue = JsonConvert.SerializeObject(value);

            await _cache.SetStringAsync(
                key,
                serializedValue,
                options,
                cancellationToken);
        }

        public async Task DeleteAsync(
            string key,
            CancellationToken cancellationToken)
        {
            await _cache.RemoveAsync(
                key,
                cancellationToken);
        }
    }
}
