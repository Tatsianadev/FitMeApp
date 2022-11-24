using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
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

            CurrentDayEventsViewModel model = new CurrentDayEventsViewModel()
            {
                Year = year,
                Month = month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month),
                Day = 0,
                DayName = null,
                Events = null
            };
           
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View(model);            
        }


        //Caledar - PartialView

        public IActionResult Calendar()
        {            
            return PartialView();
        }

        public IActionResult CalendarCarousel(int year, int month)
        {    
            CurrentDayEventsViewModel model = new CurrentDayEventsViewModel()
            {
                Year = year,
                Month = month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                Day = 0,
                DayName = null,
                Events = null
            };
           
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View("Index", model);
        }


        //Events - PartialView
        public IActionResult ShowEvents(int year, int month, int day)
        {
            CurrentDayEventsViewModel model = new CurrentDayEventsViewModel()
            {
                Year = year,
                Month = month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                Day = day,
                DayName = new DateTime(year,month,day).DayOfWeek.ToString(),
                Events = null
            };
            
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View("Index", model);            
        }

        public IActionResult Events()
        {
            return PartialView();
        }
    }
}
