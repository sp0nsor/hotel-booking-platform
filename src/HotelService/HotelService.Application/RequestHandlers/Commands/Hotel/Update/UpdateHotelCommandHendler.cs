using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Update
{
    public class UpdateHotelCommandHendler : IRequestHandler<UpdateHotelCommand, Result>
    {
        private readonly IImageService imageService;
        private readonly IRedisCacheService cacheService;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public UpdateHotelCommandHendler(
            IRedisCacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService)
        {
            this.cacheService = cacheService;
            this.hotelRepository = hotelRepository;
            this.imageService = imageService;
        }

        public async Task<Result> Handle(
            UpdateHotelCommand request, 
            CancellationToken cancellationToken)
        {
            var existHotel = await hotelRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (existHotel is null)
                return Result.Failure("Hotel not found");

            var deleteOldImageTask = imageService.DeleteImageAsync(
                existHotel.Image.Value,
                cancellationToken);

            var writeImageResult = await imageService.WriteImageAsync(
                request.Image,
                cancellationToken);

            if (writeImageResult.IsFailure)
                return Result.Failure(writeImageResult.Error);

            var createHotelResult = Core.Models.Hotel.Create(
                request.Id,
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

            await hotelRepository.UpdateAsync(createHotelResult.Value, cancellationToken);

            var cachedKey = $"hotel_{request.Id}";
            await cacheService.DeleteAsync(cachedKey);

            await deleteOldImageTask;

            return Result.Success();
        }
    }
}
