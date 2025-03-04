using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.GetById
{
    public class GetRoomByIdQuery(Guid Id, Guid HotelId)
        : IRequest<Result<RoomDto>>
    {
        public Guid Id { get; set; } = Id;
        public Guid HotelId { get; set; } = HotelId;
    }
}
