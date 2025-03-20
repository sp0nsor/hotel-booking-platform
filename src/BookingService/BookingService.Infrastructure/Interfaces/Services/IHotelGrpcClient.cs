using BookingService.Infrastructure.Services.Grpc.Hotel;
using CSharpFunctionalExtensions;

namespace BookingService.Infrastructure.Interfaces.Services
{
    public interface IHotelGrpcClient
    {
        Result<GetHotelByIdResponse> GetHotelById(Guid id);
    }
}