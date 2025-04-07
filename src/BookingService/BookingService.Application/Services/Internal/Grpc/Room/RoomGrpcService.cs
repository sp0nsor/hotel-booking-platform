using BookingService.Application.Interfaces.Internal;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Services.Internal.Grpc.Room
{
    public class RoomGrpcService : IRoomGrpcService
    {
        private readonly RoomService.RoomServiceClient _client;

        public RoomGrpcService(RoomService.RoomServiceClient client)
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
