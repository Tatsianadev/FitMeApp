using FitMeApp.Services.Contracts.Interfaces;
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
            //try
            //{
            //    var eventsByUser = _scheduleService.GetEventsByUser("7122f67c-d670-47d4-8afb-0fd14b76cec4");
            //    var eventsByUserAndDate = _scheduleService.GetEventsByUserAndDate("afb2a4c0-7ff4-400c-98c1-448908b39e46", new DateTime(2022, 12, 8));
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            


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

        public IActionResult ShowEvents(int year, int month, int day)
        {
            return View();
        }
    }
}
