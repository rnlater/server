using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using Shared.Types;
using Shared.Constants;

namespace Application.Services
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Result<string>> SendEmail(string to, string name, string subject, string body)
        {
            var message = new MimeMessage();
            var senderName = _configuration["Smtp:Name"];
            var senderEmail = _configuration["Smtp:Email"];
            message.From.Add(new MailboxAddress(senderName, senderEmail));
            message.To.Add(new MailboxAddress(name, to));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    var smtpServer = _configuration["Smtp:Server"];
                    var smtpPort = int.Parse(_configuration["Smtp:Port"]);
                    var smtpPass = _configuration["Smtp:Pass"];

                    await client.ConnectAsync(smtpServer, smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
                    await client.AuthenticateAsync(senderEmail, smtpPass);

                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);

                    return Result<string>.Done(to);
                }
                catch (Exception)
                {
                    return Result<string>.Fail(ErrorMessage.EmailNotSent);
                }
            }
        }
    }
}