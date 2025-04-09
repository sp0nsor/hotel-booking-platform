using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using BookingService.Application.Interfaces.Internal;
using BookingService.Application.Options;

namespace BookingService.Application.Services.Internal
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
            var email = CreateEmailMessage(recipientEmail, subject, body);

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls, cancellationToken);
            await smtp.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password, cancellationToken);
            await smtp.SendAsync(email, cancellationToken);
            await smtp.DisconnectAsync(true, cancellationToken);
        }

        private MimeMessage CreateEmailMessage(string recipientEmail, string subject, string body)
        {
            return new MimeMessage
            {
                From = { MailboxAddress.Parse(_emailOptions.UserName) },
                To = { MailboxAddress.Parse(recipientEmail) },
                Subject = subject,
                Body = new TextPart(TextFormat.Text) { Text = body }
            };
        }
    }
}
