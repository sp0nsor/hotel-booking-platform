using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Update
{
    public class UpdateRoomCommandHandler : IRequestHandler<UpdateRoomCommand, Result>
    {
        private readonly IImageService imageService;
        private readonly IRedisCacheService cacheService;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;
        private readonly IRepository<Core.Models.Room> roomRepository;

        public UpdateRoomCommandHandler(
            IImageService imageService,
            IRedisCacheService cacheService,
            IRepository<Core.Models.Hotel> hotelRepository,
            IRepository<Core.Models.Room> roomRepository)
        {
            this.imageService = imageService;
            this.cacheService = cacheService;
            this.hotelRepository = hotelRepository;
            this.roomRepository = roomRepository;
        }

        public async Task<Result> Handle(
            UpdateRoomCommand request, 
            CancellationToken cancellationToken)
        {
            var existRoom = await roomRepository.GetByIdAsync(
                request.Id,
                cancellationToken);

            if (existRoom is null)
                return Result.Failure("Room not found");

            var deleteOldImageTask = imageService.DeleteImageAsync(
                existRoom.Image.Value,
                cancellationToken);

            var imageResult = await imageService.WriteImageAsync(request.Image, cancellationToken);

            if (imageResult.IsFailure)
                return Result.Failure(imageResult.Error);

            var roomResult = Core.Models.Room.Create(
                request.Id,
                request.HotelId,
                request.Capacity,
                request.Area,
                request.Number,
                request.MoneyAmount,
                request.Currency,
                imageResult.Value);

            if(roomResult.IsFailure)
                return Result.Failure(roomResult.Error);

            await roomRepository.UpdateAsync(roomResult.Value, cancellationToken);

            await deleteOldImageTask;

            var cachedKey = $"room_{request.Id}";
            await cacheService.DeleteAsync(cachedKey);

            return Result.Success();
        }
    }
}
