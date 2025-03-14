using BookingService.Infrastructure.Data.Specifications;
using BookingService.Infrastructure.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace BookingService.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(BookingDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetSingleAsync(
            Specification<T> specification,
            CancellationToken cancellationToken)
        {
            var entity = await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(specification.ToExpression(), cancellationToken);

            return entity;
        }

        public virtual async Task<(IEnumerable<T> Items, int TotalPages)> GetAsync(
            Specification<T> specification,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var totalCount = await _dbSet.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var items = await _dbSet
                .AsNoTracking()
                .Where(specification.ToExpression())
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalPages);
        }

        public virtual async Task CreateAsync(
            T entity,
            CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(
            T entity,
            CancellationToken cancellationToken)
        {
            _dbSet.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
