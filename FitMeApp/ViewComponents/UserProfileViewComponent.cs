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
        public IViewComponentResult Invoke()
        {
            return View("UserProfile");
        }
    }
}
