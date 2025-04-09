using BookingService.Application.Services.Internal.Grpc.Room;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Interfaces.Internal
{
    public interface IRoomGrpcService
    {
        Result<GetRoomByIdResponse> GetRoomById(Guid roomId, Guid hotelId);
    }
}
