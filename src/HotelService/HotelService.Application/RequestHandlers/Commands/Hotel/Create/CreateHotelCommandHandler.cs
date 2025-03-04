using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Create
{
    public class CreateHotelCommandHandler
        : IRequestHandler<CreateHotelCommand, Result>
    {
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;
        private readonly IImageService _imageService;

        public CreateHotelCommandHandler(
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService)
        {
            _hotelRepository = hotelRepository;
            _imageService = imageService;
        }

        public async Task<Result> Handle(
            CreateHotelCommand request,
            CancellationToken cancellationToken)
        {
            var imagePath = await _imageService.WriteImageAsync(
                request.Image,
                cancellationToken);

            var createHotelResult = Core.Models.Hotel.Create(
                Guid.NewGuid(),
                request.Name,
                request.Description,
                request.PhoneNumber,
                request.Country,
                request.City,
                request.Street,
                request.PriceCategory,
                imagePath
            );

            if (createHotelResult.IsFailure)
            {
                await _imageService.DeleteImageAsync(
                    imagePath,
                    CancellationToken.None);

                return Result.Failure(createHotelResult.Error);
            }

            await _hotelRepository.AddAsync(createHotelResult.Value, cancellationToken);

            return Result.Success();
        }
    }
}
