namespace UserService.Application.Requests
{
    public record UpdateUserInfoRequest(
        string FirstName,
        string LastName,
        string PhoneNumber);
}
