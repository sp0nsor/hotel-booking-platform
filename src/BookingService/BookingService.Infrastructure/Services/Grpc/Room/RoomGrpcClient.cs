using BookingService.Infrastructure.Interfaces.Services;
using CSharpFunctionalExtensions;
using Grpc.Net.Client;

namespace BookingService.Infrastructure.Services.Grpc.Room
{
    public class RoomGrpcClient : IRoomGrpcClient
    {
        private readonly RoomService.RoomServiceClient _client;

        public RoomGrpcClient(RoomService.RoomServiceClient client)
        {
            _client = client;
        }

        public Result<GetRoomByIdResponse> GetRoomById(Guid roomId, Guid hotelId)
        {
            var response = _client.GetRoomById(
                new GetRoomByIdRequest
                {
                    RoomId = roomId.ToString(),
                    HotelId = hotelId.ToString()
                });

            if (!string.IsNullOrEmpty(response.ErrorMessage))
                return Result.Failure<GetRoomByIdResponse>(response.ErrorMessage);

            return response;
        }
    }
}
