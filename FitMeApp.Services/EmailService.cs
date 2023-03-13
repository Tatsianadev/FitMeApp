using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace FitMeApp.Services
{
    public class EmailService: IEmailService
    {
        private string _apiKey;

        public EmailService(string apiKey)
        {
            _apiKey = apiKey;   
        }

        public async Task SendEmailAsync(string toEmail, string fromEmail, string apiKey, string subject, string plainTextContent, string htmlContent)
        {
            var to = new EmailAddress(toEmail, "receiver");
            var from = new EmailAddress(fromEmail, "sender");
            var client = new SendGridClient(apiKey);
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var responce = await client.SendEmailAsync(message);
        }
    }
}
