using FitMeApp.Common;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class TrainerProfileViewComponent: ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IGymService _fitMeService;

        public TrainerProfileViewComponent(UserManager<User> userManager, IGymService fitMeService)
        {
            _userManager = userManager;
            _fitMeService = fitMeService;
        }


        public IViewComponentResult Invoke()
        {
            ViewBag.OpenedEventsCount = 2; //test
            ViewBag.ClientsToAddCount = 5; //test
            ViewBag.NotificationCount = 7; //test
            return View("TrainerProfile");
        }
    }
}
