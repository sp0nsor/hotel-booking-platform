using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Create
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, Result>
    {
        private readonly IRepository<Core.Models.Room> roomRepository;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;
        private readonly IImageService imageService;

        public CreateRoomCommandHandler(
            IRepository<Core.Models.Room> roomRepository,
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService)
        {
            this.roomRepository = roomRepository;
            this.hotelRepository = hotelRepository;
            this.imageService = imageService;
        }

        public async Task<Result> Handle(
            CreateRoomCommand request,
            CancellationToken cancellationToken)
        {
            var getHotelTask = hotelRepository.GetByIdAsync(
                request.HotelId,
                cancellationToken,
                includeProperties: "Rooms");

            var writeImageResult = await imageService
                .WriteImageAsync(request.Image, cancellationToken);
            if(writeImageResult.IsFailure)
                return Result.Failure(writeImageResult.Error);

            var hotel = await getHotelTask;
            if (hotel is null)
            {
                await imageService.DeleteImageAsync(
                    writeImageResult.Value,
                    cancellationToken);

                return Result.Failure("Hotel not found");
            }

            var existRoom = hotel.Rooms?.FirstOrDefault(r => r.Number == request.Number);
            if (existRoom != null)
            {
                await imageService.DeleteImageAsync(
                    writeImageResult.Value,
                    cancellationToken);

                return Result.Failure("Room whith this number exist");
            }

            var createRoomResult = Core.Models.Room.Create(
                Guid.NewGuid(),
                request.HotelId,
                request.Capacity,
                request.Area,
                request.Number,
                request.MoneyAmount,
                request.Currency,
                writeImageResult.Value);

            if(createRoomResult.IsFailure)
                return Result.Failure(createRoomResult.Error);

            await roomRepository.AddAsync(
                createRoomResult.Value,
                cancellationToken);

            return Result.Success();
        }
    }
}
