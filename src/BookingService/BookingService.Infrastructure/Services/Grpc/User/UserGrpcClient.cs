using BookingService.Infrastructure.Interfaces.Services;
using CSharpFunctionalExtensions;

namespace BookingService.Infrastructure.Services.Grpc.User
{
    public class UserGrpcClient : IUserGrpcClient
    {
        private readonly UserService.UserServiceClient _client;

        public UserGrpcClient(UserService.UserServiceClient client)
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
