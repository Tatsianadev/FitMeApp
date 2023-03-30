using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.WEB.Contracts.ViewModels.Chart;
using Microsoft.AspNetCore.Mvc;

namespace FitMeApp.ViewComponents
{
    public class VisitingChartBySelectedDayOfWeekViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(VisitingChartViewModel modelBySelectedDay)
        {
            return View("VisitingChartBySelectedDayOfWeek", modelBySelectedDay);
        }
    }
}
