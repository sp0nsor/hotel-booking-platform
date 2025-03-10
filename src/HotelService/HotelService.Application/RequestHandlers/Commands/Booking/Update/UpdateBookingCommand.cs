using CSharpFunctionalExtensions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Booking.Update
{
    public class UpdateBookingCommand : IRequest<Result>
    {
        public Guid Id { get; set; }
        public Guid HotelId { get; set; }
        public Guid RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
