using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using HotelService.Core.Common;
using MediatR;

namespace HotelService.Application.Queries.Hotel.Get
{
    public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, PaginatedResult<HotelDto>>
    {
        private readonly IHotelRepository hotelRepository;

        public GetHotelsQueryHandler(IHotelRepository hotelRepository)
        {
            this.hotelRepository = hotelRepository;
        }

        public async Task<PaginatedResult<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
        {
            var hotelsPage = await hotelRepository.Get(
                request.PageIndex,
                request.PageSize,
                cancellationToken);

            // mapping to hotel dto

            return new PaginatedResult<HotelDto>();
        }
    }
}
