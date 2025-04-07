namespace UserService.Application.Options
{
    public class AccessTokenOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public int ExpiresMinutes { get; set; }
    }
}
