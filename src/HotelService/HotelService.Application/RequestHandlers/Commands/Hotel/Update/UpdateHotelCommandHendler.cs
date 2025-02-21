using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Update
{
    public class UpdateHotelCommandHendler : IRequestHandler<UpdateHotelCommand, Result>
    {
        private readonly IImageService imageService;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public UpdateHotelCommandHendler(
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService)
        {
            this.hotelRepository = hotelRepository;
            this.imageService = imageService;
        }

        public async Task<Result> Handle(
            UpdateHotelCommand request, 
            CancellationToken cancellationToken)
        {
            var imageResult = await imageService.WriteImage(
                request.Image,
                cancellationToken);

            if (imageResult.IsFailure)
                return Result.Failure(imageResult.Error);

            var hotelResult = Core.Models.Hotel.Create(
                request.Id,
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

            await hotelRepository.UpdateAsync(hotelResult.Value, cancellationToken);

            return Result.Success();
        }
    }
}
