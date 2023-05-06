using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Quartz;

namespace FitMeApp.Services.QuartzService
{
    [DisallowConcurrentExecution]
    public class SendEmailJob : IJob
    {
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;
        private readonly ILogger<SendEmailJob> _logger;

        public SendEmailJob(IRepository repository, IEmailService emailService, ILogger<SendEmailJob> logger)
        {
            _repository = repository;
            _emailService = emailService;
            _logger = logger;
        }


        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => SendEmailToTargetUsersAsync());
            return task;
        }

        public async void SendEmailToTargetUsersAsync()
        {
            int daysBeforeSubscrExpire = Convert.ToInt32(Common.DefaultSettingsStorage.SendEmailJobIfDays); 
            var targetUsers = _repository.GetAllUsersByExpiringSubscriptions(daysBeforeSubscrExpire).ToList();
            string fromEmail = DefaultSettingsStorage.SenderEmail;
            string subject = "Subscription expire";

            foreach (var user in targetUsers)
            {
                string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study case - constant
                string plainTextContent = $"Your {user.GymName} subscription will expire in {daysBeforeSubscrExpire} days.";
                string htmlContent = $"<strong>Your {user.GymName} subscription will expire in {daysBeforeSubscrExpire} days.</strong>";
               
                try
                {
                    await _emailService.SendEmailAsync(toEmail, user.UserFirstName, fromEmail, subject, plainTextContent, htmlContent);
                    _logger.LogInformation($"Email to {user.UserFirstName} was sent.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"UserId: {user.UserId} failed SendEmail attempt. " + ex.Message);
                }
            }
        }

    }
}
