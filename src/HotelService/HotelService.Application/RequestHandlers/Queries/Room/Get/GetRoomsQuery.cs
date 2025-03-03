using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.Get
{
    public class GetRoomsQuery(Guid HotelId, int PageIndex = 1, int PageSize = 10)
        : IRequest<PaginatedResult<RoomDto>>
    {
        public Guid HotelId { get; set; } = HotelId;
        public int PageIndex { get; set; } = PageIndex;
        public int PageSize { get; set; } = PageSize;
    }
}
