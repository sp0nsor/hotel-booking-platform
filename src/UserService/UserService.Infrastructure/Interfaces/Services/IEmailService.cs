namespace UserService.Infrastructure.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body, CancellationToken cancellationToken);
    }
}