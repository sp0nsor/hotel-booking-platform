using AutoMapper;
using HotelService.Core.Models;
using HotelService.Core.Abstractions;
using HotelService.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Hotel>> Get(
            CancellationToken cancellationToken)
        {
            var hotelEntities = await context.Hotels
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var hotels = mapper.Map<List<Hotel>>(hotelEntities);

            return hotels;
        }

        public async Task<Hotel> GetById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var hotelEntity = await context.Hotels
                .Include(h => h.Rooms)
                .ThenInclude(r => r.BookedDateEntities)
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
