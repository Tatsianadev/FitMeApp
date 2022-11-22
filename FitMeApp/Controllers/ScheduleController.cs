using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ILogger _logger;

        public ScheduleController(IScheduleService scheduleService, ILoggerFactory loggerFactory)
        {
            _scheduleService = scheduleService;
            _logger = loggerFactory.CreateLogger("ScheduleController");
        }




        public IActionResult Index()
        {
            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;
            Dictionary<string, int> calendarData = new Dictionary<string, int>()
            {
                {"year", year },
                { "month", month}
            };

            ViewBag.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View(calendarData);

            
        }

        public IActionResult Calendar()
        {
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return PartialView();
        }

        public IActionResult CalendarCarousel(int year, int month)
        {           
            Dictionary<string, int> calendarData = new Dictionary<string, int>()
            {
                {"year", year },
                { "month", month}
            };

            ViewBag.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View("Index", calendarData);
        }
    }
}
