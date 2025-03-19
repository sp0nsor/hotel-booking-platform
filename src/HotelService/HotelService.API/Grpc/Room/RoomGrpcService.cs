using AutoMapper;
using Grpc.Core;
using HotelService.Application.RequestHandlers.Queries.Room.GetById;
using MediatR;

namespace HotelService.API.Grpc.Room
{
    public class RoomGrpcService 
        : RoomService.RoomServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public RoomGrpcService(
            IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public override async Task<GetRoomByIdResponse> GetRoomById(
            GetRoomByIdRequest request,
            ServerCallContext context)
        {
            var query = new GetRoomByIdQuery(
                Guid.Parse(request.RoomId),
                Guid.Parse(request.HotelId));

            var result = await _mediator.Send(query, CancellationToken.None);

            if(result.IsFailure)
                return new GetRoomByIdResponse { ErrorMessage = result.Error };

            return _mapper.Map<GetRoomByIdResponse>(result.Value);
        }
    }
}
