using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using BookingService.Infrastructure.Interfaces.Services;

namespace BookingService.Infrastructure.Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly EmailNotificationOptions _notificationOptions;

        public EmailService(IOptions<EmailNotificationOptions> options)
        {
            _notificationOptions = options.Value;
        }

        public async Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string body,
            CancellationToken cancellationToken)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_notificationOptions.UserName));
            email.To.Add(MailboxAddress.Parse(recipientEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Text) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_notificationOptions.Host, _notificationOptions.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_notificationOptions.UserName, _notificationOptions.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
