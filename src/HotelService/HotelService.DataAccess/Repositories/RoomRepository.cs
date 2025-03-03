using AutoMapper;
using HotelService.Core.Models;
using HotelService.Core.Abstractions;
using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelService.DataAccess.Repositories
{
    public class RoomRepository : Repository<Room, RoomEntity>, IRoomRepository
    {
        public RoomRepository(HotelDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<Room?> GetByIdAsync(
            Guid id,
            Guid hotelId,
            CancellationToken cancellationToken)
        {
            var roomEntity = await _dbSet
                .AsNoTracking()
                .Where(r => r.Id == id && r.HotelId == hotelId)
                .Include(r => r.BookingPeriods)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<Room>(roomEntity);
        }

        public async Task<(IEnumerable<Room> Items, int TotalPages)> GetAllAsync(
            Guid hotelId,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var totalCount = await _dbSet
                .Where(r => r.HotelId == hotelId)
                .CountAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var roomEntities = await _dbSet
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId)
                .OrderBy(r => r.Number)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var rooms = _mapper.Map<List<Room>>(roomEntities);

            return (rooms, totalPages);
        }
    }
}
