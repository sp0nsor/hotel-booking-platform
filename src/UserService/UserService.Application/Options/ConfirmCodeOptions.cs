namespace UserService.Application.Options
{
    public class ConfirmCodeOptions
    {
        public int CodeLength { get; set; }
        public int ExpiresMinutes { get; set; }
    }
}
