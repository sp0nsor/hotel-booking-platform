using CSharpFunctionalExtensions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Delete
{
    public class DeleteRoomCommand(Guid Id, Guid HotelId) : IRequest<Result>
    {
        public Guid Id { get; set; } = Id;
        public Guid HotelId { get; set; } = HotelId;
    }
}
