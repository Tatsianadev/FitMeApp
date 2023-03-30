using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.WEB.Contracts.ViewModels.Chart;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace FitMeApp.ViewComponents
{
    public class VisitingChartBySelectedDayOfWeekViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(VisitingChartViewModel modelBySelectedDay)
        {
            var points = modelBySelectedDay.TimeVisitorsLine;

            ViewBag.Data = JsonConvert.SerializeObject(points);
            return View("VisitingChartBySelectedDayOfWeek");
        }
    }
}
