using AutoMapper;
using HotelService.Core.Models;
using HotelService.Core.Abstractions;
using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using HotelService.Core.Common;

namespace HotelService.DataAccess.Repositories
{
    public class HotelRepository : IHotelRepository
    {
        private readonly HotelDbContext context;
        private readonly IMapper mapper;

        public HotelRepository(
            HotelDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<Guid> Create(
            Hotel hotel,
            CancellationToken cancellationToken)
        {
            var hotelEntity = mapper.Map<HotelEntity>(hotel);

            await context.Hotels.AddAsync(hotelEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return hotelEntity.Id;
        }

        public async Task<PaginatedResult<Hotel>> Get(
            int pageIndex,
            int pageSize,
            CancellationToken cancellationToken)
        {
            int skipAmount = (pageIndex - 1) * pageSize;

            var totalCount = await context.Hotels.CountAsync(cancellationToken);

            var hotelEntities = await context.Hotels
                .AsNoTracking()
                .OrderBy(h => h.Id)
                .Skip(skipAmount)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            var hotels = mapper.Map<List<Hotel>>(hotelEntities);
            
            int totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            var result = new PaginatedResult<Hotel>
            {
                Items = hotels,
                CurrentPage = pageIndex,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return result;
        }

        public async Task<Hotel> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var hotelEntity = await context.Hotels
                .Include(h => h.Rooms)
                    .ThenInclude(r => r.BookedDates)
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

            var hotel = mapper.Map<Hotel>(hotelEntity);

            return hotel;
        }

        public async Task<Guid> Update(
            Hotel hotel,
            CancellationToken cancellationToken)
        {
            var hotelEntity = mapper.Map<HotelEntity>(hotel);

            context.Hotels.Update(hotelEntity);
            await context.SaveChangesAsync(cancellationToken);

            return hotelEntity.Id;
        }

        public async Task<Guid> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var hotelEntity = context.Hotels
                .FirstOrDefault(h => h.Id == id);

            if (hotelEntity == null)
                return Guid.Empty;

            context.Hotels.Remove(hotelEntity);
            await context.SaveChangesAsync(cancellationToken);

            return hotelEntity.Id;
        }
    }
}
