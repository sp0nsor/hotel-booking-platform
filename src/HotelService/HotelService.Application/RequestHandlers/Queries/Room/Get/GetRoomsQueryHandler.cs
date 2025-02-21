using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.Get
{
    public class GetRoomsQueryHandler : IRequestHandler<GetRoomsQuery, Result<List<RoomDto>>>
    {
        private readonly IMapper mapper;
        private readonly IRepository<Core.Models.Hotel> hotelRepository;
        private readonly IRepository<Core.Models.Room> roomRepository;

        public GetRoomsQueryHandler(
            IMapper mapper,
            IRepository<Core.Models.Hotel> hotelRepository,
            IRepository<Core.Models.Room> roomRepository)
        {
            this.mapper = mapper;
            this.hotelRepository = hotelRepository;
            this.roomRepository = roomRepository;
        }

        public async Task<Result<List<RoomDto>>> Handle(
            GetRoomsQuery request, 
            CancellationToken cancellationToken)
        {
            var hotel = await hotelRepository.GetByIdWithIncludeAsync(
                request.HotelId,
                cancellationToken,
                includeProperties: "Rooms");

            if (hotel is null)
                return Result.Failure<List<RoomDto>>("Hotel not found");

            var roomDtos = mapper.Map<List<RoomDto>>(hotel.Rooms);

            return Result.Success(roomDtos);
        }
    }
}
