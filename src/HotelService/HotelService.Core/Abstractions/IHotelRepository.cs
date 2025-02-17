using HotelService.Core.Common;
using HotelService.Core.Models;

namespace HotelService.Core.Abstractions
{
    public interface IHotelRepository
    {
        Task<Guid> Create(Hotel hotel, CancellationToken cancellationToken);
        Task<Guid> Delete(Guid id, CancellationToken cancellationToken);
        Task<PaginatedResult<Hotel>> Get(int pageIndex, int pageSize, CancellationToken cancellationToken);
        Task<Hotel> GetById(Guid id, CancellationToken cancellationToken);
        Task<Guid> Update(Hotel hotel, CancellationToken cancellationToken);
    }
}