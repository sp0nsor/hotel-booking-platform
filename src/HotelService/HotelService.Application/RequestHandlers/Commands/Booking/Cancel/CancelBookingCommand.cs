using CSharpFunctionalExtensions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Booking.Cancel
{
    public class CancelBookingCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid RoomId { get; set; }
    }
}
