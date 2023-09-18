using FitMeApp.Common;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class TrainerProfileViewComponent : ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IScheduleService _scheduleService;

        public TrainerProfileViewComponent(UserManager<User> userManager, IScheduleService scheduleService)
        {
            _userManager = userManager;
            _scheduleService = scheduleService;
        }


        public IViewComponentResult Invoke()
        {
            var trainerId = _userManager.GetUserId((ClaimsPrincipal)User);
            ViewBag.OpenedEventsCount = _scheduleService.GetOpenedEventsCountByTrainer(trainerId);
            //ViewBag.NewMesseges = 5;
            return View("TrainerProfile");
        }
    }
}
