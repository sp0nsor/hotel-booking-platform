using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.Get
{
    public record GetHotelsQuery(
        int PageIndex = 1,
        int PageSize = 10) : IRequest<PaginatedResult<HotelDto>>;
}
