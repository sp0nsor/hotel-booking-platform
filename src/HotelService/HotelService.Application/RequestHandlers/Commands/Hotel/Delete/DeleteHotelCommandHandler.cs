using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Delete
{
    public class DeleteHotelCommandHandler 
        : IRequestHandler<DeleteHotelCommand, Result>
    {
        private readonly IImageService _imageService;
        private readonly ICacheService _cacheService;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;

        public DeleteHotelCommandHandler(
            IImageService imageService,
            ICacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            _imageService = imageService;
            _cacheService = cacheService;
            _hotelRepository = hotelRepository;
        }

        public async Task<Result> Handle(
            DeleteHotelCommand request,
            CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (hotel is null)
                return Result.Failure("Hotel not found");

            var cachedKey = $"hotel_{request.Id}";

            var deleteImageTask = _imageService.DeleteImageAsync(
                hotel.Image.Value, 
                cancellationToken);

            var deleteHotelTask = _hotelRepository.DeleteAsync(
                hotel, 
                cancellationToken);

            var deleteCacheTask =  _cacheService.DeleteAsync(
                cachedKey,
                cancellationToken);

            await Task.WhenAll(deleteImageTask, deleteHotelTask, deleteCacheTask);

            return Result.Success();
        }
    }
}
