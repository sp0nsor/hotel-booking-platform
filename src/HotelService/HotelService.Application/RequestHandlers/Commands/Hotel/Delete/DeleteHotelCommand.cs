using CSharpFunctionalExtensions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Delete
{
    public class DeleteHotelCommand(Guid Id) : IRequest<Result>
    {
        public Guid Id { get; set; } = Id;
    }
}
