using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.Get
{
    public record GetRoomsQuery(
        Guid HotelId) : IRequest<Result<List<RoomDto>>>;
}
