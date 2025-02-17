using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.Commands.Hotel.UpdateHotel
{
    public class UpdateHotelCommandHendler : IRequestHandler<UpdateHotelCommand, Result<Guid>>
    {
        private readonly IHotelRepository hotelRepository;
        private readonly IImageService imageService;

        public UpdateHotelCommandHendler(
            IHotelRepository hotelRepository,
            IImageService imageService)
        {
            this.hotelRepository = hotelRepository;
            this.imageService = imageService;
        }

        public async Task<Result<Guid>> Handle(UpdateHotelCommand request, CancellationToken cancellationToken)
        {
            var imageResult = await imageService.WriteImage(
                request.Image,
                cancellationToken);

            if (imageResult.IsFailure)
                return Result.Failure<Guid>(imageResult.Error);

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
                return Result.Failure<Guid>(hotelResult.Error);

            var id = await hotelRepository.Update(hotelResult.Value, cancellationToken);

            return Result.Success(id);
        }
    }
}
