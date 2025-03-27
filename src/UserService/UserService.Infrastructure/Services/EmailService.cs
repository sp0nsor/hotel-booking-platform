using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;

using MailKit.Security;
using UserService.Infrastructure.Options;
using UserService.Infrastructure.Interfaces.Services;

namespace UserService.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailOptions _emailOptions;

        public EmailService(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public async Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string body,
            CancellationToken cancellationToken)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_emailOptions.UserName));
            email.To.Add(MailboxAddress.Parse(recipientEmail));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Text) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }
    }
}
