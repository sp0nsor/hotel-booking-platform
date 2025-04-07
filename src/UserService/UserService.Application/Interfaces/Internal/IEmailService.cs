namespace UserService.Application.Interfaces.Internal
{
    public interface IEmailService
    {
        Task SendEmailAsync(string recipientEmail, string subject, string body, CancellationToken cancellationToken);
    }
}
