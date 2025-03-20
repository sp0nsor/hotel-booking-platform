using BookingService.Infrastructure.Interfaces.Services;
using CSharpFunctionalExtensions;
using Grpc.Net.Client;

namespace BookingService.Infrastructure.Services.Grpc.Hotel
{
    public class HotelGrpcClient : IHotelGrpcClient
    {
        private readonly HotelService.HotelServiceClient _client;

        public HotelGrpcClient(HotelService.HotelServiceClient client)
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
