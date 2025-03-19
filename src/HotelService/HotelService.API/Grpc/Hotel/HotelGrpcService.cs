using AutoMapper;
using Grpc.Core;
using HotelService.Application.RequestHandlers.Queries.Hotel.GetById;
using MediatR;

namespace HotelService.API.Grpc.Hotel
{
    public class HotelGrpcService
        : HotelService.HotelServiceBase
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public HotelGrpcService(
            IMapper mapper,
            IMediator mediator)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        public override async Task<GetHotelByIdResponse> GetHotelById(
            GetHotelByIdRequest request, 
            ServerCallContext context)
        {
            var query = new GetHotelByIdQuery(Guid.Parse(request.Id));

            var result = await _mediator.Send(query, CancellationToken.None);

            if (result.IsFailure)
                return new GetHotelByIdResponse { ErrorMessage = result.Error };

            return _mapper.Map<GetHotelByIdResponse>(result.Value);
        }
    }
}
