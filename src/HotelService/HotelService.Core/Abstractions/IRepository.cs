using CSharpFunctionalExtensions;

namespace HotelService.Core.Abstractions
{
    public interface IRepository<TDomain> where TDomain : class
    {
        Task<TDomain?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TDomain?> GetByIdWithIncludeAsync(Guid id, CancellationToken cancellationToken = default, params string[] includeProperties);
        Task<(IEnumerable<TDomain> Items, int TotalPages)> GetAllAsync(int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task AddAsync(TDomain entity, CancellationToken cancellationToken = default);
        Task UpdateAsync(TDomain entity, CancellationToken cancellationToken = default);
        Task DeleteAsync(TDomain entity, CancellationToken cancellationToken = default);
    }
}
