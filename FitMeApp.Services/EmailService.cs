using System;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace FitMeApp.Services
{
    public sealed class EmailService: IEmailService
    {
        private string _apiKey;

        public EmailService(string apiKey)
        {
            _apiKey = apiKey;   
        }

        public async Task SendEmailAsync(string toEmail, string toUserName, string fromEmail, string subject, string plainTextContent, string htmlContent)
        {
            var to = new EmailAddress(toEmail, toUserName);
            var from = new EmailAddress(fromEmail, Common.DefaultSettingsStorage.ApplicationName);
            var client = new SendGridClient(_apiKey);
            var message = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(message);
        }
    }
}
