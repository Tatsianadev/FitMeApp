using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FitMeApp.ViewComponents
{
    public class GroupClassesPerDayOnScheduleCalendarViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(List<GroupClassScheduleViewModel> schedulePerDayInfo)
        {
            return View("GroupClassesPerDayOnScheduleCalendar", schedulePerDayInfo);
        }
    }
}
