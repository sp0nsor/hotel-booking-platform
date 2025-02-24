using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Delete
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand>
    {
        private readonly IImageService imageService;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public DeleteHotelCommandHandler(
            IImageService imageService,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            this.imageService = imageService;
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
        }
    }
}
