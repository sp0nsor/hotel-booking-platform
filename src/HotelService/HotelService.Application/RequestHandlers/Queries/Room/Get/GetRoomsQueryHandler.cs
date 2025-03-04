using AutoMapper;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.Get
{
    public class GetRoomsQueryHandler 
        : IRequestHandler<GetRoomsQuery, PaginatedResult<RoomDto>>
    {
        private readonly IMapper _mapper;
        private readonly IRoomRepository _roomRepository;
        private readonly IRepository<Core.Models.Hotel> _hotelRepository;

        public GetRoomsQueryHandler(
            IMapper mapper,
            IRoomRepository roomRepository,
            IRepository<Core.Models.Hotel> hotelRepository)
        {
            _mapper = mapper;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
        }

        public async Task<PaginatedResult<RoomDto>> Handle(
            GetRoomsQuery request, 
            CancellationToken cancellationToken)
        {
            var (rooms, totalPages) = await _roomRepository.GetAllAsync(
                request.HotelId,
                request.PageIndex,
                request.PageSize, 
                cancellationToken);

            var roomsDto = _mapper.Map<List<RoomDto>>(rooms);

            var paginatedResult = new PaginatedResult<RoomDto>
            {
                Items = roomsDto,
                PageSize = request.PageSize,
                CurrentPage = request.PageIndex,
                TotalPages = totalPages
            };

            return paginatedResult;
        }
    }
}
