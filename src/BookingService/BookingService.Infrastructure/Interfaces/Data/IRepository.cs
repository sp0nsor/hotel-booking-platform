using BookingService.Infrastructure.Data.Specifications;

namespace BookingService.Infrastructure.Interfaces.Data
{
    public interface IRepository<T> where T : class
    {
        Task CreateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
        Task<(IEnumerable<T> Items, int TotalPages)> GetAsync(Specification<T> specification, int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<T?> GetSingleAsync(Specification<T> specification, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
    }
}