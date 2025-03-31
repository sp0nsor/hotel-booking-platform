namespace UserService.Infrastructure.Options
{
    public class RefreshTokenOptions
    {
        public int ExpiresDays { get; set; }
        public int TokenLength { get; set; }
    }
}
