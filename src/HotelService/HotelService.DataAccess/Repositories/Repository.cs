using AutoMapper;
using HotelService.Core.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace HotelService.DataAccess.Repositories
{
    public class Repository<TDomain, TEntity> : IRepository<TDomain>
        where TDomain : class
        where TEntity : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<TEntity> _dbSet;
        protected readonly IMapper _mapper;

        public Repository(HotelDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
            _mapper = mapper;
        }

        public virtual async Task<TDomain?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken,
            params string[] includeProperties)
        {
            var query = _dbSet.AsQueryable();

            foreach (var includeProperty in includeProperties)
                query = query.Include(includeProperty);

            var entity = await query
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);

            return entity != null ? _mapper.Map<TDomain>(entity) : null;
        }

        public virtual async Task<(IEnumerable<TDomain> Items, int TotalPages)> GetAllAsync(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var totalCount = await _dbSet.CountAsync(cancellationToken);
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var entities = await _dbSet
                .AsNoTracking()
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var items = _mapper.Map<IEnumerable<TDomain>>(entities);

            return (Items: items, TotalPages: totalPages);
        }

        public virtual async Task AddAsync(
            TDomain domain,
            CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(domain);

            await _dbSet.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task UpdateAsync(
            TDomain domain,
            CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(domain);
            
            _dbSet.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }

        public virtual async Task DeleteAsync(
            TDomain domain,
            CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(domain);

            _dbSet.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
