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
    public class UserProfileViewComponent: ViewComponent
    {
        private readonly UserManager<User> _userManager;
        private readonly IGymService _fitMeService;

        public UserProfileViewComponent(UserManager<User> userManager, IGymService fitMeService)
        {
            _userManager = userManager;
            _fitMeService = fitMeService;
        }


        public IViewComponentResult Invoke()
        {
            ViewBag.ChangedEventsCount = 2; //test
            ViewBag.NotificationCount = 2; //test
            return View("UserProfile");
        }
    }
}
