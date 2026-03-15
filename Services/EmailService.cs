using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace GestionFerias_CTPINVU.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string bodyHtml)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Sistema de Ferias INVU", "no-reply@invu.cr"));
            email.To.Add(new MailboxAddress("", toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = bodyHtml };
            email.Body = builder.ToMessageBody();

            var host = _config["SmtpSettings:Host"];
            var port = int.Parse(_config["SmtpSettings:Port"] ?? "587");
            var user = _config["SmtpSettings:Username"];
            var pass = _config["SmtpSettings:Password"];

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(host, port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(user, pass);
            
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
