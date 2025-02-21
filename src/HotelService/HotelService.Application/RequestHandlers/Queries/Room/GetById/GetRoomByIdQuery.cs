using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.GetById
{
    public record GetRoomByIdQuery(Guid Id) : IRequest<Result<RoomDto>>;
}
