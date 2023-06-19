using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class DietController : Controller
    {
        public IActionResult WelcomeToDiedPlan()
        {
            return View();
        }
    }
}
