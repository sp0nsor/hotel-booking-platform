using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Hotel.Get
{
    public class GetHotelsQueryHandler : IRequestHandler<GetHotelsQuery, PaginatedResult<HotelDto>>
    {
        private readonly IMapper mapper;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;

        public GetHotelsQueryHandler(
            IMapper mapper,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            this.mapper = mapper;
            this.hotelRepository = hotelRepository;
        }

        public async Task<PaginatedResult<HotelDto>> Handle(
            GetHotelsQuery request, 
            CancellationToken cancellationToken)
        {
            var (items, totalPages) = await hotelRepository.GetAllAsync(
                request.PageIndex,
                request.PageSize,
                cancellationToken);

            var hotelDtos = mapper.Map<List<HotelDto>>(items);

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
