using BookingService.Application.Interfaces.Internal;
using CSharpFunctionalExtensions;

namespace BookingService.Application.Services.Internal.Grpc.User
{
    public class UserGrpcService : IUserGrpcService
    {
        private readonly UserService.UserServiceClient _client;

        public UserGrpcService(UserService.UserServiceClient client)
        {
            _client = client;
        }

        public Result<GetUserByIdResponse> GetUserById(Guid id)
        {
            var response = _client.GetUserById(
                new GetUserByIdRequest { Id = id.ToString() });

            if (!string.IsNullOrEmpty(response.ErrorMessage))
                return Result.Failure<GetUserByIdResponse>(response.ErrorMessage);

            return response;
        }
    }
}
