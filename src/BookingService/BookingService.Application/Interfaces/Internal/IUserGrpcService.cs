using BookingService.Application.Services.Internal.Grpc.User;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Interfaces.Internal
{
    public interface IUserGrpcService
    {
        Result<GetUserByIdResponse> GetUserById(Guid id);
    }
}
