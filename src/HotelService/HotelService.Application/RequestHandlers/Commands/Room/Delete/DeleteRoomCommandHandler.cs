using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Delete
{
    public class DeleteRoomCommandHandler
        : IRequestHandler<DeleteRoomCommand, Result>
    {
        private readonly ICacheService _cacheService;
        private readonly IImageService _imageService;
        private readonly IRoomRepository _roomRepository;

        public DeleteRoomCommandHandler(
            ICacheService cacheService,
            IImageService imageService,
            IRoomRepository roomRepository)
        {
            _cacheService = cacheService;
            _imageService = imageService;
            _roomRepository = roomRepository;
        }

        public async Task<Result> Handle(
            DeleteRoomCommand request,
            CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetByIdAsync(
                request.Id,
                request.HotelId,
                cancellationToken);

            if (room is null)
                return Result.Failure("Room not found");

            var cachedKey = $"room_{request.Id}";

            var deleteImageTask = _imageService.DeleteImageAsync(
                room.Image.Value,
                cancellationToken);

            var deleteRoomTask = _roomRepository.DeleteAsync(room, cancellationToken);

            var deleteCacheTask = _cacheService.DeleteAsync(
                cachedKey,
                cancellationToken);

            await Task.WhenAll(deleteRoomTask, deleteCacheTask, deleteImageTask);

            return Result.Success();
        }
    }
}
