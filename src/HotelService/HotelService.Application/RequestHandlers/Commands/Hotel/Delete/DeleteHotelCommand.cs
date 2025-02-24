using CSharpFunctionalExtensions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Delete
{
    public record DeleteHotelCommand(Guid Id) : IRequest;
}
