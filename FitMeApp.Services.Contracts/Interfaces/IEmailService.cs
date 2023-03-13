using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string fromEmail, string subject, string plainTextContent,
            string htmlContent);
    }
}
