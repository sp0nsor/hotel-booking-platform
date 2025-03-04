using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Create
{
    public class CreateRoomCommandHandler
        : IRequestHandler<CreateRoomCommand, Result>
    {
        private readonly IRepository<Core.Models.Room> _roomRepository;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;
        private readonly IImageService _imageService;

        public CreateRoomCommandHandler(
            IRepository<Core.Models.Room> roomRepository,
            IRepository<Core.Models.Hotel> hotelRepository,
            IImageService imageService)
        {
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _imageService = imageService;
        }

        public async Task<Result> Handle(
            CreateRoomCommand request,
            CancellationToken cancellationToken)
        {
            var hotel = await _hotelRepository.GetByIdAsync(
                request.HotelId,
                cancellationToken,
                includeProperties: "Rooms");

            if (hotel is null)
                return Result.Failure("Hotel not found");

            var existingRoom = hotel.Rooms?.FirstOrDefault(r => r.Number == request.Number);

            if (existingRoom != null)
                return Result.Failure("Room whith this number exist");

            var imagePath = await _imageService
                .WriteImageAsync(request.Image, cancellationToken);

            var createRoomResult = Core.Models.Room.Create(
                Guid.NewGuid(),
                request.HotelId,
                request.Capacity,
                request.Area,
                request.Number,
                request.MoneyAmount,
                request.Currency,
                imagePath);

            if(createRoomResult.IsFailure)
            {
                await _imageService.DeleteImageAsync(
                    imagePath,
                    CancellationToken.None);

                return Result.Failure(createRoomResult.Error);
            }

            await _roomRepository.AddAsync(
                createRoomResult.Value,
                cancellationToken);

            return Result.Success();
        }
    }
}
