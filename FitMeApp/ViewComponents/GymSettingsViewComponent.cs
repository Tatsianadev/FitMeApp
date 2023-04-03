using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class GymSettingsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("GymSettings");
        }
    }
}
