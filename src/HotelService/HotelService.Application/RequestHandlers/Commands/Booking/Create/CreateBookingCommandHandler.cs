using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using HotelService.Core.Models;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Booking.Create
{
    public class CreateBookingCommandHandler
        : IRequestHandler<CreateBookingCommand, Result>
    {
        private readonly ICacheService _cacheService;
        private readonly IBookingPeriodRepository _bookingPeriodRepository;
        private readonly IRoomRepository _roomRepository;

        public CreateBookingCommandHandler(
            ICacheService cacheService,
            IBookingPeriodRepository bookingRepository,
            IRoomRepository roomRepository)
        {
            _cacheService = cacheService;
            _bookingPeriodRepository = bookingRepository;
            _roomRepository = roomRepository;
        }

        public async Task<Result> Handle(
            CreateBookingCommand request, 
            CancellationToken cancellationToken)
        {
            var room = await _roomRepository.GetByIdAsync(
                request.RoomId,
                request.HotelId, 
                cancellationToken);

            if (room is null)
                return Result.Failure("Room not found");

            var createBookingResult = BookingPeriod.Create(
                request.Id,
                request.RoomId,
                request.StartDate,
                request.EndDate);

            if (createBookingResult.IsFailure)
                return Result.Failure(createBookingResult.Error);

            if (room.BookingPeriods.Any(createBookingResult.Value.Overlaps))
                return Result.Failure("Conflict booking dates");

            await _bookingPeriodRepository.AddAsync(
                createBookingResult.Value,
                cancellationToken);

            var cacheKey = $"room_{room.Id}";

            await _cacheService.DeleteAsync(cacheKey, cancellationToken);

            return Result.Success();
        }
    }
}
