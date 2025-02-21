using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Create
{
    public class CreateHotelCommandHandler : IRequestHandler<CreateHotelCommand, Result>
    {
        private readonly IRepository<Core.Models.Hotel> hotelRepository;
        private readonly IImageService imageService;

        public CreateHotelCommandHandler(
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService)
        {
            this.hotelRepository = hotelRepository;
            this.imageService = imageService;
        }

        public async Task<Result> Handle(
            CreateHotelCommand request,
            CancellationToken cancellationToken)
        {
            var imageResult = await imageService.WriteImage(
                request.Image,
                cancellationToken);

            if (imageResult.IsFailure)
                return Result.Failure(imageResult.Error);

            var hotelResult = Core.Models.Hotel.Create(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.PhoneNumber,
                request.Country,
                request.City,
                request.Street,
                request.PriceCategory,
                imageResult.Value
            );

            if (hotelResult.IsFailure)
                return Result.Failure(hotelResult.Error);

            await hotelRepository.AddAsync(hotelResult.Value, cancellationToken);

            return Result.Success();
        }
    }
}
