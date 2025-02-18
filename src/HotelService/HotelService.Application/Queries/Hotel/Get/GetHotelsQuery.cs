using HotelService.Application.DTOs;
using HotelService.Core.Common;
using MediatR;

namespace HotelService.Application.Queries.Hotel.Get
{
    public record GetHotelsQuery(
        int PageIndex = 1,
        int PageSize = 10) : IRequest<PaginatedResult<HotelDto>>;
}
