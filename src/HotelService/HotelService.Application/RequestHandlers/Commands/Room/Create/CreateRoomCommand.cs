using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.RequestHandlers.Commands.Room.Create
{
    public class CreateRoomCommand : IRequest<Result>
    {
        public Guid HotelId { get; set; }
        public int Area { get; set; }
        public int Number { get; set; }
        public int Capacity { get; set; }
        public decimal MoneyAmount { get; set; }
        public string Currency { get; set; }
        public IFormFile Image { get; set; }
    }
}
