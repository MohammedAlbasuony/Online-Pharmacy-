using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;
namespace Pharmacy.BLL.Service.Implementation
{
    public class SmtpEmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailSender> _logger;

        public SmtpEmailSender(IConfiguration configuration, ILogger<SmtpEmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpSettings = _configuration.GetSection("Smtp");

            using (var client = new SmtpClient())
            {
                client.Host = smtpSettings["Host"];
                client.Port = int.Parse(smtpSettings["Port"]);
                client.EnableSsl = bool.Parse(smtpSettings["EnableSsl"]);
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential(
                    smtpSettings["Username"],
                    smtpSettings["Password"]
                );
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"], smtpSettings["FromName"]),
                    Subject = subject,
                    Body = htmlMessage,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                try
                {
                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation($"Email sent to {email}");
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
