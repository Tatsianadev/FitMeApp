using FitMeApp.Common;
using FitMeApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FitMeApp.Services.Contracts.Interfaces;


namespace FitMeApp.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly IScheduleService _scheduleService;

        public HomeController(IEmailService emailService, IScheduleService scheduleService)
        {
            _emailService = emailService;
            _scheduleService = scheduleService;
        }

        public IActionResult Index()
        {
            SendEmailToTargetUsersAsync();
            return View();
        }


        //TEST (delete later)
        public async void SendEmailToTargetUsersAsync()
        {
            int numDaysToSubscriptionExpire = 3;
            var targetUsers = _scheduleService.GetAllUsersByExpiringSubscriptions(numDaysToSubscriptionExpire);
            //foreach (var user in targetUsers)
            //{
            //    string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study case - constant
            //    string fromEmail = DefaultSettingsStorage.SenderEmail;
            //    string plainTextContent = $"Your {user.GymName} subscription will expire in {numDaysToSubscriptionExpire} days.";
            //    string htmlContent = $"<strong>Your {user.GymName} subscription will expire in {numDaysToSubscriptionExpire} days.</strong>";
            //    string subject = "Subscription expire";

            //    await _emailService.SendEmailAsync(toEmail, user.UserFirstName, fromEmail, subject, plainTextContent, htmlContent);
            //}
        }
        //TEST end


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
