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
            var writeImageResult = await imageService.WriteImageAsync(
                request.Image,
                cancellationToken);

            if (writeImageResult.IsFailure)
                return Result.Failure(writeImageResult.Error);

            var createHotelResult = Core.Models.Hotel.Create(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.PhoneNumber,
                request.Country,
                request.City,
                request.Street,
                request.PriceCategory,
                writeImageResult.Value
            );

            if (createHotelResult.IsFailure)
                return Result.Failure(createHotelResult.Error);

            await hotelRepository.AddAsync(createHotelResult.Value, cancellationToken);

            return Result.Success();
        }
    }
}
