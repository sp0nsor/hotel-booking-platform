using BookingService.Infrastructure.Interfaces.Services;
using CSharpFunctionalExtensions;
using Grpc.Net.Client;

namespace BookingService.Infrastructure.Services.Grpc.Hotel
{
    public class HotelGrpcClient : IHotelGrpcClient
    {
        private readonly HotelService.HotelServiceClient _client;

        public HotelGrpcClient()
        {
            var httpHandler = new HttpClientHandler();
            httpHandler.ServerCertificateCustomValidationCallback = 
                HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            var channel = GrpcChannel.ForAddress(
                "https://hotel-service:8081",
                new GrpcChannelOptions
            {
                HttpHandler = httpHandler
            });

            _client = new HotelService.HotelServiceClient(channel);
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
