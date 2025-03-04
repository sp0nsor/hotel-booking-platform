using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Application.Interfaces;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.GetById
{
    public class GetRoomByIdQueryHandler
        : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
    {
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;
        private readonly IRoomRepository _roomRepository;

        public GetRoomByIdQueryHandler(
            IMapper mapper,
            ICacheService cacheService,
            IRoomRepository roomRepository)
        {
            _mapper = mapper;
            _cacheService = cacheService;
            _roomRepository = roomRepository;
        }

        public async Task<Result<RoomDto>> Handle(
            GetRoomByIdQuery request, 
            CancellationToken cancellationToken)
        {
            var cachedKey = $"room_{request.Id}";

            var cachedRoom = await _cacheService.GetAsync<RoomDto>(
                cachedKey,
                cancellationToken);

            if(cachedRoom != null)
                return Result.Success(cachedRoom);

            var room = await _roomRepository.GetByIdAsync(
                request.Id,
                request.HotelId,
                cancellationToken);

            if (room is null)
                return Result.Failure<RoomDto>("Room not found");

            var roomDto = _mapper.Map<RoomDto>(room);

            await _cacheService.SetAsync(
                cachedKey, 
                roomDto,
                cancellationToken,
                TimeSpan.FromDays(1));
            
            return Result.Success(roomDto);
        }
    }
}
