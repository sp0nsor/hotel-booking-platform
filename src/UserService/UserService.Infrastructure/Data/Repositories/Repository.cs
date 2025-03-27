using Microsoft.EntityFrameworkCore;
using UserService.Infrastructure.Data.Specifications;
using UserService.Infrastructure.Interfaces.Data;
namespace UserService.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public Repository(UsersDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<T?> GetSingleAsync(
            Specification<T> specification,
            CancellationToken cancellationToken)
        {
            return await specification.Includes
                .Aggregate(
                    _dbSet.AsNoTracking().Where(specification.ToExpression()),
                    (query, include) => query.Include(include))
                .FirstOrDefaultAsync(cancellationToken);
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

        public virtual async Task DeleteAsync(
            T entity,
            CancellationToken cancellationToken)
        {
            _dbSet.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
