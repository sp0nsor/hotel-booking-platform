using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Create
{
    public record CreateHotelCommand(
        string Name,
        string Description,
        string PhoneNumber,
        string Country,
        string City,
        string Street,
        string PriceCategory,
        IFormFile Image) : IRequest<Result>;
}
