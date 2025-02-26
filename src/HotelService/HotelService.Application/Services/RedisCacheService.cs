using HotelService.Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace HotelService.Application.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache cache;

        public RedisCacheService(
            IDistributedCache cache)
        {
            this.cache = cache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await cache.GetStringAsync(key);

            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiry
            };

            var serializedValue = JsonConvert.SerializeObject(value);

            await cache.SetStringAsync(key, serializedValue, options);
        }

        public async Task DeleteAsync(string key)
        {
            await cache.RemoveAsync(key);
        }
    }
}
