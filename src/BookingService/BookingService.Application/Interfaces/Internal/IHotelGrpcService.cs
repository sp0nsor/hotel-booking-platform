using BookingService.Application.Services.Internal.Grpc.Hotel;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Interfaces.Internal
{
    public interface IHotelGrpcService
    {
        Result<GetHotelByIdResponse> GetHotelById(Guid id);
    }
}
