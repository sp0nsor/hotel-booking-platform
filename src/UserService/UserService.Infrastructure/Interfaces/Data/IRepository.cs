using UserService.Infrastructure.Data.Specifications;

namespace UserService.Infrastructure.Interfaces.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T?> GetSingleAsync(Specification<T> specification, CancellationToken cancellationToken);
        Task CreateAsync(T entity, CancellationToken cancellationToken);
        Task UpdateAsync(T entity, CancellationToken cancellationToken);
        Task DeleteAsync(T entity, CancellationToken cancellationToken);
    }
}