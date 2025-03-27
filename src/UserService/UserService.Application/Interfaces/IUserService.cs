using CSharpFunctionalExtensions;
using UserService.Application.DTOs;
using UserService.Application.Requests;

namespace UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<Result> ConfirmUserAsync(ConfirmUserRequest confirmUserRequest, CancellationToken cancellationToken);
        Task<Result<LoginDto>> LoginUserAsync(LoginUserRequest loginUserRequest, CancellationToken cancellationToken);
        Task<Result> LogoutUserAsync(string jwtTokenId, string refreshTokenValue, CancellationToken cancellationToken);
        Task<Result<LoginDto>> RefreshUserTokenAsync(string refreshTokenValue, CancellationToken cancellationToken);
        Task<Result> RegisterUserAsync(RegisterUserRequest registerUserRequest, CancellationToken cancellationToken);
        Task<Result> UpdateUserInfoAsync(Guid userId, UpdateUserInfoRequest updateUserInfoRequest, CancellationToken cancellationToken);
    }
}