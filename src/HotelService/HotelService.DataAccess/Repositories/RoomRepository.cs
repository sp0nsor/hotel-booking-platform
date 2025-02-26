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
            var roomEntity = await dbSet
                .AsNoTracking()
                .Where(r => r.Id == id && r.HotelId == hotelId)
                .Include(r => r.BookedDates)
                .FirstOrDefaultAsync(cancellationToken);

            var room = mapper.Map<Room>(roomEntity);

            return room;
        }

        public async Task<(IEnumerable<Room> Items, int TotalPages)> GetAllAsync(
            Guid hotelId,
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken)
        {
            var totalCount = await dbSet
                .Where(r => r.HotelId == hotelId)
                .CountAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var roomEntities = await dbSet
                .AsNoTracking()
                .Where(r => r.HotelId == hotelId)
                .OrderBy(r => r.Number)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var rooms = mapper.Map<List<Room>>(roomEntities);

            return (rooms, totalPages);
        }
    }
}
