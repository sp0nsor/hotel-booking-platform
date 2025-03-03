using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.GetById
{
    public class GetHotelByIdQuery(Guid Id) : IRequest<Result<HotelDto>>
    {
        public Guid Id { get; set; } = Id;
    }
}
