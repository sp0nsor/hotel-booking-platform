using CSharpFunctionalExtensions;
using MediatR;

namespace HotelService.Application.Commands.Hotel.DeleteHotel
{
    public record DeleteHotelCommand(Guid id) : IRequest<Result<Guid>>;
}
