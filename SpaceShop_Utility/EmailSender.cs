using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;

namespace SpaceShop_Utility
{
    public class EmailSender : IEmailSender
    {

        readonly IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return ExecuteGmail(email, subject, htmlMessage);
        }
        public async Task ExecuteGmail(string email, string subject, string htmlMessage)
        {
            var fromAddress = new MailAddress(PathManager.EmailSender, PathManager.EmailSenderName);
            var toAddress = new MailAddress(email);
            MailJetSettings settings = configuration.GetSection("MailJet").Get<MailJetSettings>();
            string fromPassword = settings.GmailSecretKey;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            })
            {
                smtp.Send(message);
            }
        }
    }
}
