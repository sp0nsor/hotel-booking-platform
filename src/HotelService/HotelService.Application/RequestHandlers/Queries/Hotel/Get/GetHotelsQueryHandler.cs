using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.Get
{
    public class GetHotelsQueryHandler 
        : IRequestHandler<GetHotelsQuery, PaginatedResult<HotelDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;

        public GetHotelsQueryHandler(
            IMapper mapper,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            _mapper = mapper;
            _hotelRepository = hotelRepository;
        }

        public async Task<PaginatedResult<HotelDto>> Handle(
            GetHotelsQuery request, 
            CancellationToken cancellationToken)
        {
            var (items, totalPages) = await _hotelRepository.GetAllAsync(
                request.PageIndex,
                request.PageSize,
                cancellationToken);

            var hotelDtos = _mapper.Map<List<HotelDto>>(items);

            var paginatedResult = new PaginatedResult<HotelDto>
            {
                Items = hotelDtos,
                PageSize = request.PageSize,
                CurrentPage = request.PageIndex,
                TotalPages = totalPages
            };

            return paginatedResult;
        }
    }
}
