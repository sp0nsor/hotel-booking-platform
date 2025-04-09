using UserService.Application.Interfaces.Internal;

namespace UserService.Application.Services.Internal
{
    public class PasswordService : IPasswordService
    {
        public string Generate(string password) =>
            BCrypt.Net.BCrypt.EnhancedHashPassword(password);

        public bool Verify(string password, string passwordHash) =>
            BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
    }
}
