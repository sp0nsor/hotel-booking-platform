using AutoMapper;
using Grpc.Core;
using UserService.Application.Interfaces.Public;

namespace UserService.API.Grpc
{
    public class UserGrpcService 
        : UserService.UserServiceBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserGrpcService(IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        public override async Task<GetUserByIdResponse> GetUserById(
            GetUserByIdRequest request,
            ServerCallContext context)
        {
            var result = await _userService.GetUserByIdAsync(
                Guid.Parse(request.Id), 
                CancellationToken.None);

            if (result.IsFailure)
                return new GetUserByIdResponse { ErrorMessage = result.Error };

            return _mapper.Map<GetUserByIdResponse>(result.Value);
        }
    }
}
