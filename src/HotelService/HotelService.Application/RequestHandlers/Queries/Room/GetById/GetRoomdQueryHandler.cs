using AutoMapper;
using CSharpFunctionalExtensions;
using HotelService.Application.DTOs;
using HotelService.Core.Abstractions;
using MediatR;

namespace HotelService.Application.RequestHandlers.Queries.Room.GetById
{
    public class GetRoomdQueryHandler : IRequestHandler<GetRoomByIdQuery, Result<RoomDto>>
    {
        private readonly IMapper mapper;
        private readonly IRoomRepository roomRepository;

        public GetRoomdQueryHandler(
            IMapper mapper,
            IRoomRepository roomRepository)
        {
            this.mapper = mapper;
            this.roomRepository = roomRepository;
        }

        public async Task<Result<RoomDto>> Handle(
            GetRoomByIdQuery request, 
            CancellationToken cancellationToken)
        {
            var room = await roomRepository.GetByIdAsync(
                request.Id,
                request.HotelId,
                cancellationToken);

            if (room is null)
                return Result.Failure<RoomDto>("Room not found");

            var roomDto = mapper.Map<RoomDto>(room);
            
            return Result.Success(roomDto);
        }
    }
}
