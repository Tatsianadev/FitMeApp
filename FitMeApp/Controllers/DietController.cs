using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace FitMeApp.Controllers
{
    public class DietController : Controller
    {
        public IActionResult WelcomeToDietPlan()
        {
            return View();
        }


        [Authorize]
        public IActionResult AnthropometricInfo()
        {
            return View(new AnthropometricInfoViewModel());
        }


    }
}
