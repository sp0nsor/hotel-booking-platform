using HotelService.Application.DTOs;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.Get
{
    public class GetHotelsQuery(int PageIndex = 1, int PageSize = 10) 
        : IRequest<PaginatedResult<HotelDto>>
    {
        public int PageIndex { get; set; } = PageIndex;
        public int PageSize { get; set; } = PageSize;
    }
}
