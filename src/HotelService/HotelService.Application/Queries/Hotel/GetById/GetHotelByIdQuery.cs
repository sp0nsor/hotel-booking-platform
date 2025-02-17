using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.Queries.Hotel.GetById
{
    public record GetHotelByIdQuery(Guid Id) : IRequest<Result<HotelDto>>;
}
