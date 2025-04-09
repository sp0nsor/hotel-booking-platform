namespace UserService.Application.Requests
{
    public record LoginUserRequest(
        string Email,
        string Password);
}
