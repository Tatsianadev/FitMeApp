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
            var data = modelBySelectedDay.TimeVisitorsLine;
            var dataPoints = new List<TimeVisitorsAsChartDataPointViewModel>();
            foreach (var point in data)
            {
               dataPoints.Add(new TimeVisitorsAsChartDataPointViewModel()
               {    
                  NumberOfVisitors = point.NumberOfVisitors,
                  Hour = point.Hour.ToString() + ".00"
               });
            }

            ViewBag.Data = JsonConvert.SerializeObject(dataPoints);
            return View("VisitingChartBySelectedDayOfWeek");
        }
    }
}
