using CSharpFunctionalExtensions;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Booking.Cancel
{
    public class CancelBookingCommandHandler
        : IRequestHandler<CancelBookingCommand, Result>
    {
        private readonly ICacheService _cacheService;
        private readonly IBookingRepository _bookingRepository;

        public CancelBookingCommandHandler(
            ICacheService cacheService,
            IBookingRepository bookingRepository)
        {
            _cacheService = cacheService;
            _bookingRepository = bookingRepository;
        }

        public async Task<Result> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
        {
            var booking = await _bookingRepository.GetByIdAsync(
                request.Id,
                request.RoomId,
                cancellationToken);

            if (booking is null)
                return Result.Failure("Booking not found");

            await _bookingRepository.DeleteAsync(
                booking,
                cancellationToken);

            var cacheKey = $"room_{request.RoomId}";

            await _cacheService.DeleteAsync(
                cacheKey, 
                cancellationToken);

            return Result.Success();
        }
    }
}
