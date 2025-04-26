using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Update
{
    public class UpdateRoomCommandHandler 
        : IRequestHandler<UpdateRoomCommand, Result<RoomDto>>
    {
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        private readonly ICacheService _cacheService;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;
        private readonly IRepository<Core.Models.Room> _roomRepository;

        public UpdateRoomCommandHandler(
            IImageService imageService,
            ICacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository,
            IRepository<Core.Models.Room> roomRepository,
            IMapper mapper)
        {
            _imageService = imageService;
            _cacheService = cacheService;
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _mapper = mapper;
        }

        public async Task<Result<RoomDto>> Handle(
            UpdateRoomCommand request, 
            CancellationToken cancellationToken)
        {
            var existRoom = await _roomRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (existRoom is null)
                return Result.Failure<RoomDto>("Room not found");

            string imagePath;

            if(request.Image is null)
            {
                imagePath = existRoom.Image.Value;
            }
            else
            {
                imagePath = await _imageService.WriteImageAsync(
                    request.Image, 
                    cancellationToken);

                await _imageService.DeleteImageAsync(
                    existRoom.Image.Value,
                    cancellationToken);
            }

            var createRoomResult = Core.Models.Room.Create(
                request.Id,
                request.HotelId,
                request.Capacity,
                request.Area,
                request.Number,
                request.MoneyAmount,
                request.Currency,
                imagePath);

            if (createRoomResult.IsFailure)
            {
                await _imageService.DeleteImageAsync(
                    imagePath,
                    CancellationToken.None);

                return Result.Failure<RoomDto>(createRoomResult.Error);
            }

            var deleteRoomTask = _roomRepository.UpdateAsync(
                createRoomResult.Value, 
                cancellationToken);

            var cachedKey = $"room_{request.Id}";

            var deleteCacheTask = _cacheService.DeleteAsync(
                cachedKey,
                cancellationToken);

            await Task.WhenAll(deleteCacheTask, deleteRoomTask);

            return Result.Success(_mapper.Map<RoomDto>(createRoomResult.Value));
        }
    }
}
