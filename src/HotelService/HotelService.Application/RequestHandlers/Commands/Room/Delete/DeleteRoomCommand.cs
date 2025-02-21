using CSharpFunctionalExtensions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Room.Delete
{
    public record DeleteRoomCommand(Guid Id) : IRequest;
}
