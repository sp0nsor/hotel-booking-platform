using BookingService.Infrastructure.Services.Grpc.Room;
using CSharpFunctionalExtensions;

namespace BookingService.Infrastructure.Interfaces.Services
{
    public interface IRoomGrpcClient
    {
        Result<GetRoomByIdResponse> GetRoomById(Guid roomId, Guid hotelId);
    }
}