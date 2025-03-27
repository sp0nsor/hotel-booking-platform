namespace UserService.Infrastructure.Interfaces.Services
{
    public interface ICacheService
    {
        Task DeleteAsync(string key, CancellationToken cancellationToken);
        Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);
        Task SetAsync<T>(string key, T value, CancellationToken cancellationToken, TimeSpan? expiry = null);
    }
}