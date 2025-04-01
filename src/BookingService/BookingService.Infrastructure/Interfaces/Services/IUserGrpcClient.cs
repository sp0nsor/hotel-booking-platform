using BookingService.Infrastructure.Services.Grpc.User;
using CSharpFunctionalExtensions;

namespace BookingService.Infrastructure.Interfaces.Services
{
    public interface IUserGrpcClient
    {
        Result<GetUserByIdResponse> GetUserById(Guid id);
    }
}