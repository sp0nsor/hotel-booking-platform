using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace HotelService.Application.RequestHandlers.Commands.Hotel.Update
{
    public class UpdateHotelCommand : IRequest<Result<HotelDto>>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string PriceCategory { get; set; }
        public IFormFile? Image { get; set; } = null;
    }
}
