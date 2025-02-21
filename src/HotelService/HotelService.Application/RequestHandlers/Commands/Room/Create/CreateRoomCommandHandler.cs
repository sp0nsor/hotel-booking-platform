using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            var hotelTask = hotelRepository.GetByIdAsync(
                request.HotelId,
                cancellationToken,
                includeProperties: "Rooms");

            var imageResult = await imageService.WriteImage(request.Image, cancellationToken);
            if(imageResult.IsFailure)
                return Result.Failure<Guid>(imageResult.Error);

            var hotel = await hotelTask;
            if (hotel is null)
                return Result.Failure("Hotel not found");

            var existRoom = hotel.Rooms.FirstOrDefault(r => r.Number == request.Number);
            if (existRoom != null)
                return Result.Failure("Room whith this number exist");

            var roomResult = Core.Models.Room.Create(
                Guid.NewGuid(),
                request.HotelId,
                request.Capacity,
                request.Area,
                request.Number,
                request.MoneyAmount,
                request.Currency,
                imageResult.Value);

            if(roomResult.IsFailure)
                return Result.Failure(roomResult.Error);

            await roomRepository.AddAsync(
                roomResult.Value,
                cancellationToken);

            return Result.Success();
        }
    }
}
