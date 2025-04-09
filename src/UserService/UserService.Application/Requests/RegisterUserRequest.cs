namespace UserService.Application.Requests
{
    public record RegisterUserRequest(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string PhoneNumber);
}
