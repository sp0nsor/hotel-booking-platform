namespace HotelService.Application.Interfaces
{
    public interface IRedisCacheService
    {
        Task DeleteAsync(string key);
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    }
}