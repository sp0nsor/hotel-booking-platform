using BookingService.Application.Interfaces.Internal;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Services.Internal.Grpc.Hotel
{
    public class HotelGrpcService : IHotelGrpcService
    {
        private readonly HotelService.HotelServiceClient _client;

        public HotelGrpcService(HotelService.HotelServiceClient client)
        {
            _client = client;
        }

        public Result<GetHotelByIdResponse> GetHotelById(Guid id)
        {
            var response = _client.GetHotelById(
                new GetHotelByIdRequest { Id = id.ToString() } );

            if (!string.IsNullOrEmpty(response.ErrorMessage))
                return Result.Failure<GetHotelByIdResponse>(response.ErrorMessage);

            return response;
        }
    }
}
