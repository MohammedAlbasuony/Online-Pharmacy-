using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Pharmacy.BLL.Service.Implementation
{
    public class MailtrapEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MailtrapEmailSender> _logger;

        public MailtrapEmailSender(IConfiguration configuration, ILogger<MailtrapEmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpSettings = _configuration.GetSection("Mailtrap");

            using (var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"])))
            {
                client.Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]);
                client.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("noreply@pharmacare.org", "PharmaCare"),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                try
                {
                    await client.SendMailAsync(mailMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Failed to send email to {email}");
                    throw;
                }
            }
        }
    }
}
