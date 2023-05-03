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
using Quartz;

namespace FitMeApp.Services.SchedulerService
{
    public class SendEmailTask : IJob
    {
        private readonly IRepository _repository;
        private readonly IEmailService _emailService;

        public SendEmailTask(IRepository repository, IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }



        //public Task Execute(IJobExecutionContext context)
        //{
        //    var task = Task.Run(() => SendEmailToTargetUsersAsync());
        //    return task;
        //}

        //private async void SendEmailToTargetUsersAsync()
        //{
        //    int numDaysToSubscriptionExpire = 3;
        //    var targetUsers = _repository.GetAllUsersByExpiringSubscriptions(numDaysToSubscriptionExpire).ToList();
        //    foreach (var user in targetUsers)
        //    {
        //        string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study case - constant
        //        string fromEmail = DefaultSettingsStorage.SenderEmail;
        //        string plainTextContent = $"Your {user.GymName} subscription will expire in {numDaysToSubscriptionExpire} days.";
        //        string htmlContent = $"<strong>Your {user.GymName} subscription will expire in {numDaysToSubscriptionExpire} days.</strong>";
        //        string subject = "Subscription expire";

        //        await _emailService.SendEmailAsync(toEmail, user.UserFirstName, fromEmail, subject, plainTextContent, htmlContent);
        //    }
        //}


        public Task Execute(IJobExecutionContext context)
        {
            var task = Task.Run(() => LogFile(DateTime.Now));
            return task;
        }

        public void LogFile(DateTime time)
        {
            string path = @"c:\tatsiana\stud\SelfCodingPractice\SquadTestApp\FitMeAppTest.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(time);
                writer.Close();
            }
        }
    }
}
