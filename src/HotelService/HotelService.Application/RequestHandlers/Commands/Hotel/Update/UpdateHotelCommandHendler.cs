using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Update
{
    public class UpdateHotelCommandHendler
        : IRequestHandler<UpdateHotelCommand, Result>
    {
        private readonly IImageService _imageService;
        private readonly ICacheService _cacheService;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;

        public UpdateHotelCommandHendler(
            ICacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService)
        {
            _cacheService = cacheService;
            _hotelRepository = hotelRepository;
            _imageService = imageService;
        }

        public async Task<Result> Handle(
            UpdateHotelCommand request, 
            CancellationToken cancellationToken)
        {
            var existHotel = await _hotelRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (existHotel is null)
                return Result.Failure("Hotel not found");

            string imagePath;

            if(request.Image is null)
            {
                imagePath = existHotel.Image.Value;
            }
            else
            {
                imagePath = await _imageService.WriteImageAsync(
                request.Image,
                cancellationToken);

                await _imageService.DeleteImageAsync(
                    existHotel.Image.Value,
                    cancellationToken);
            }

            var createHotelResult = Core.Models.Hotel.Create(
                request.Id,
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
                    cancellationToken);

                return Result.Failure(createHotelResult.Error);
            }

            var cachedKey = $"hotel_{request.Id}";

            var deleteHotelTask = _hotelRepository.UpdateAsync(
                createHotelResult.Value,
                cancellationToken);

            var deleteCacheTask = _cacheService.DeleteAsync(
                cachedKey,
                cancellationToken);

            await Task.WhenAll(deleteCacheTask, deleteHotelTask);

            return Result.Success();
        }
    }
}
