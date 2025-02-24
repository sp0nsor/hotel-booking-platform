using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Update
{
    public record UpdateHotelCommand(
        Guid Id,
        string Name,
        string Description,
        string PhoneNumber,
        string Country,
        string City,
        string Street,
        string PriceCategory,
        IFormFile Image) : IRequest<Result>;
}
