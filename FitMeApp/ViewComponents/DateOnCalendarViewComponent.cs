using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class DateOnCalendarViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(CalendarPageWithEventsViewModel model, int dayNumber,  string showEventsActionName)
        {
            ViewBag.DayNumber = dayNumber;
            ViewBag.ShowEventsActionName = showEventsActionName;
            return View("DateOnCalendar", model);
        }
    }
}
