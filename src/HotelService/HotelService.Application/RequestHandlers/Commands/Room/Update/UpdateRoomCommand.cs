using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.RequestHandlers.Commands.Room.Update
{
    public record UpdateRoomCommand(
        Guid Id,
        Guid HotelId,
        int Area,
        int Number,
        int Capacity,
        decimal MoneyAmount,
        string Currency,
        IFormFile Image) : IRequest<Result>;
}
