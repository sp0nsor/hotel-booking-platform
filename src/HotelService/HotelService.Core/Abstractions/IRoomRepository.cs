using HotelService.Core.Models;

namespace HotelService.Core.Abstractions
{
    public interface IRoomRepository : IRepository<Room>
    {
        Task<(IEnumerable<Room> Items, int TotalPages)> GetAllAsync(Guid hotelId, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
        Task<Room?> GetByIdAsync(Guid id, Guid hotelId, CancellationToken cancellationToken = default);
    }
}