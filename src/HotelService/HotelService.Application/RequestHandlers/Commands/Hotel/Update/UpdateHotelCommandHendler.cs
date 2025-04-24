using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Update
{
    public class UpdateHotelCommandHendler
        : IRequestHandler<UpdateHotelCommand, Result<HotelDto>>
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ICacheService _cacheService;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;

        public UpdateHotelCommandHendler(
            ICacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService,
            IMapper mapper)
        {
            _cacheService = cacheService;
            _hotelRepository = hotelRepository;
            _imageService = imageService;
            _mapper = mapper;
        }

        public async Task<Result<HotelDto>> Handle(
            UpdateHotelCommand request, 
            CancellationToken cancellationToken)
        {
            var existHotel = await _hotelRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (existHotel is null)
                return Result.Failure<HotelDto>("Hotel not found");

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

                return Result.Failure<HotelDto>(createHotelResult.Error);
            }

            var deleteHotelTask = _hotelRepository.UpdateAsync(
                createHotelResult.Value,
                cancellationToken);

            var cachedKey = $"hotel_{request.Id}";

            var deleteCacheTask = _cacheService.DeleteAsync(
                cachedKey,
                cancellationToken);

            await Task.WhenAll(deleteCacheTask, deleteHotelTask);

            return Result.Success(_mapper.Map<HotelDto>(createHotelResult.Value));
        }
    }
}
