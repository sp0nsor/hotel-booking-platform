using AutoMapper;
using HotelService.Core.Abstractions;
using HotelService.Core.Models;
using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelService.DataAccess.Repositories
{
    public class BookingPeriodRepository
        : Repository<BookingPeriod, BookingPeriodEntity>, IBookingPeriodRepository
    {
        public BookingPeriodRepository(HotelDbContext context, IMapper mapper) : base(context, mapper)
        {
        }

        public async Task<BookingPeriod?> GetByIdAsync(
            Guid bookingId,
            Guid roomId,
            CancellationToken cancellationToken)
        {
            var bookingEntity = await _dbSet
                .AsNoTracking()
                .Where(b => b.RoomId == roomId && b.Id == bookingId)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<BookingPeriod>(bookingEntity);
        }
    }
}
