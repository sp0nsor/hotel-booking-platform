using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.GetById
{
    public class GetRoomdQueryHandler : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
    {
        private readonly IMapper mapper;
        private readonly IRedisCacheService cacheService;
        private readonly IRoomRepository roomRepository;

        public GetRoomdQueryHandler(
            IMapper mapper,
            IRedisCacheService cacheService,
            IRoomRepository roomRepository)
        {
            this.mapper = mapper;
            this.cacheService = cacheService;
            this.roomRepository = roomRepository;
        }

        public async Task<Result<RoomDto>> Handle(
            GetRoomByIdQuery request, 
            CancellationToken cancellationToken)
        {
            var cachedKey = $"room_{request.Id}";
            var cachedRoom = await cacheService.GetAsync<RoomDto>(cachedKey);

            if(cachedRoom != null)
                return Result.Success(cachedRoom);

            var room = await roomRepository.GetByIdAsync(
                request.Id,
                request.HotelId,
                cancellationToken);

            if (room is null)
                return Result.Failure<RoomDto>("Room not found");

            var roomDto = mapper.Map<RoomDto>(room);

            await cacheService.SetAsync(
                cachedKey, 
                roomDto,
                TimeSpan.FromDays(1));
            
            return Result.Success(roomDto);
        }
    }
}
