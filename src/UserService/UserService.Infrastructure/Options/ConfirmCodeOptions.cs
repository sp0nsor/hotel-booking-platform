namespace UserService.Infrastructure.Options
{
    public class ConfirmCodeOptions
    {
        public int CodeLength { get; set; }
        public int ExpiresMinutes { get; set; }
    }
}
