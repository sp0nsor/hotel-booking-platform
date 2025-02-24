using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.Get
{
    public record GetRoomsQuery(
        Guid HotelId,
        int PageIndex = 1,
        int PageSize = 10) : IRequest<PaginatedResult<RoomDto>>;
}
