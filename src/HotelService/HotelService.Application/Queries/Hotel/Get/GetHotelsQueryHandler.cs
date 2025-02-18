using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using HotelService.Core.Common;
using MediatR;

namespace HotelService.Application.Queries.Hotel.Get
{
    public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, PaginatedResult<HotelDto>>
    {
        private readonly IMapper mapper;
        private readonly IHotelRepository hotelRepository;

        public GetHotelsQueryHandler(
            IMapper mapper,
            IHotelRepository hotelRepository)
        {
            this.mapper = mapper;
            this.hotelRepository = hotelRepository;
        }

        public async Task<PaginatedResult<HotelDto>> Handle(GetHotelsQuery request, CancellationToken cancellationToken)
        {
            var hotelsPage = await hotelRepository.Get(
                request.PageIndex,
                request.PageSize,
                cancellationToken);

            var hotelDtos = mapper.Map<List<HotelDto>>(hotelsPage.Items);

            var paginatedResult = new PaginatedResult<HotelDto>
            {
                Items = hotelDtos,
                PageSize = hotelsPage.PageSize,
                CurrentPage = hotelsPage.CurrentPage,
                TotalPages = hotelsPage.TotalPages
            };

            return paginatedResult;
        }
    }
}
