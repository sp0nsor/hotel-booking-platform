namespace UserService.Infrastructure.Interfaces.Services
{
    public interface IPasswordService
    {
        string Generate(string password);
        bool Verify(string password, string passwordHash);
    }
}