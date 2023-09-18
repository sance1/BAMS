using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EightElements.Services.Models;
using Microsoft.Extensions.Options;

namespace EightElements.Services.Default
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSettings;
        public MailService(IOptions<MailSetting> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MailMessage();
            email.From = new MailAddress(_mailSettings.Mail, _mailSettings.DisplayName);
            email.Subject = mailRequest.Subject;
            email.IsBodyHtml = true;
            email.To.Add(new MailAddress(mailRequest.ToEmail));
            email.Body = mailRequest.Body;
            using var smtp = new SmtpClient()
            {
                Host = _mailSettings.Host,
                Port = _mailSettings.Port,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_mailSettings.Mail, _mailSettings.Password),
                EnableSsl = false,
            };
            await smtp.SendMailAsync(email);
        }
    }
}