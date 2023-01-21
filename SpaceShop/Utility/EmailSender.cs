using Mailjet.Client.TransactionalEmails;
using Mailjet.Client;
using Microsoft.AspNetCore.Identity.UI.Services;
using Mailjet.Client.Resources;

namespace SpaceShop.Utility
{
    public class EmailSender : IEmailSender
    {

        private IConfiguration configuration;

        public EmailSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        //Данные всегда отлавливаются при использовании конструктора, благодаря DI контейнеру

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string htmlMessage)
        {
            MailJetSettings settings = configuration.GetSection("MailJet").Get<MailJetSettings>();
            MailjetClient client = new MailjetClient(settings.ApiKey, settings.SecretKey);

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource
            };


            var emailMessage = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("viosagmir@gmail.com"))
                .WithSubject(subject)
                .WithHtmlPart(htmlMessage)
                .WithTo(new SendContact(email))
                .Build();

            var response = await client.SendTransactionalEmailAsync(emailMessage);
        }
        public async Task ExecuteGmail(string email, string subject, string htmlMessage)
        {
            //Cделать
        }
    }
}
