using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Delete
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
    {
        private readonly IImageService imageService;
        private readonly IRedisCacheService cacheService;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public DeleteHotelCommandHandler(
            IImageService imageService,
            IRedisCacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            this.imageService = imageService;
            this.cacheService = cacheService;
            this.hotelRepository = hotelRepository;
        }

        public async Task Handle(
            DeleteHotelCommand request,
            CancellationToken cancellationToken)
        {
            var hotel = await hotelRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (hotel is null)
                return;

            await imageService.DeleteImageAsync(hotel.Image.Value, cancellationToken);

            await hotelRepository.DeleteAsync(hotel, cancellationToken);

            var cachedKey = $"hotel_{request.Id}";
            await cacheService.DeleteAsync(cachedKey);
        }
    }
}
