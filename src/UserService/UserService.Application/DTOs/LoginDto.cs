namespace UserService.Application.DTOs
{
    public record LoginDto(
        string RefreshToken, 
        string AccessToken);
}
