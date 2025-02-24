using AutoMapper;
using HotelService.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HotelService.DataAccess.Repositories
{
    public class Repository<TDomain, TEntity> : IRepository<TDomain>
        where TDomain : class
        where TEntity : class
    {
        protected readonly DbContext context;
        protected readonly DbSet<TEntity> dbSet;
        protected readonly IMapper mapper;

        public Repository(HotelDbContext context, IMapper mapper)
        {
            this.context = context;
            dbSet = this.context.Set<TEntity>();
            this.mapper = mapper;
        }

        public virtual async Task<TDomain?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken,
            params string[] includeProperties)
        {
            var query = dbSet.AsQueryable();

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            var entity = await query
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);

            return entity != null ? mapper.Map<TDomain>(entity) : null;
        }

        public virtual async Task<(IEnumerable<TDomain> Items, int TotalPages)> GetAllAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var totalCount = await dbSet.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var entities = await dbSet
                .AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var items = mapper.Map<IEnumerable<TDomain>>(entities);

            return (Items: items, TotalPages: totalPages);
        }

        public virtual async Task AddAsync(
            TDomain domain,
            CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TEntity>(domain);
            await dbSet.AddAsync(entity, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(
            TDomain domain,
            CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TEntity>(domain);
            
            dbSet.Update(entity);

            await context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(
            TDomain domain,
            CancellationToken cancellationToken)
        {
            var entity = mapper.Map<TEntity>(domain);

            dbSet.Remove(entity);

            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
