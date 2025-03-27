namespace UserService.Infrastructure.Data.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public Guid? RefreshTokenId { get; set; }
        public RefreshTokenEntity? RefreshToken { get; set; }
        public int RoleId { get; set; }
        public RoleEntity? Role { get; set; }
        public bool IsActivated { get; set; } = false;
    }
}
