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
            List<EventViewModel> testEvents = new List<EventViewModel>();
            testEvents.Add(new EventViewModel()
            {
                Id = 1,
                Date = DateTime.Today.Date,
                StartTime = 60,
                EndTime = 120,
                Status = "test1"
            });
            testEvents.Add(new EventViewModel()
            {
                Id = 2,
                Date = DateTime.Today.Date,
                StartTime = 150,
                EndTime = 240,
                Status = "test2"
            });

            testEvents.Add(new EventViewModel()
            {
                Id = 3,
                Date = DateTime.Today.Date,
                StartTime = 240,
                EndTime = 270,
                Status = "test3"
            });

            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;  

            CurrentDayEventsViewModel model = new CurrentDayEventsViewModel()
            {
                Year = year,
                Month = month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month),
                Day = 0,
                DayName = null,
                Events = testEvents
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
            List<EventViewModel> testEvents = new List<EventViewModel>();
            testEvents.Add(new EventViewModel()
            {
                Id = 1,
                Date = DateTime.Today.Date,
                StartTime = 60,
                EndTime = 120,
                Status = "test1"
            });
            testEvents.Add(new EventViewModel()
            {
                Id = 2,
                Date = DateTime.Today.Date,
                StartTime = 150,
                EndTime = 240,
                Status = "test2"
            });

            testEvents.Add(new EventViewModel()
            {
                Id = 3,
                Date = DateTime.Today.Date,
                StartTime = 240,
                EndTime = 270,
                Status = "test3"
            });




            CurrentDayEventsViewModel model = new CurrentDayEventsViewModel()
            {
                Year = year,
                Month = month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                Day = 0,
                DayName = null,
                Events = testEvents
            };
           
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View("Index", model);
        }


        //Events - PartialView
        public IActionResult ShowEvents(int year, int month, int day)
        {
            List<EventViewModel> testEvents = new List<EventViewModel>();
            testEvents.Add(new EventViewModel()
            {
                Id = 1,
                Date = DateTime.Today.Date,
                StartTime = 60,
                EndTime = 120,
                Status = "test1"
            });
            testEvents.Add(new EventViewModel()
            {
                Id = 2,
                Date = DateTime.Today.Date,
                StartTime = 150,
                EndTime = 240,
                Status = "test2"
            });

            testEvents.Add(new EventViewModel()
            {
                Id = 3,
                Date = DateTime.Today.Date,
                StartTime = 240,
                EndTime = 270,
                Status = "test3"
            });




            CurrentDayEventsViewModel model = new CurrentDayEventsViewModel()
            {
                Year = year,
                Month = month,
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                Day = day,
                DayName = new DateTime(year,month,day).DayOfWeek.ToString(),
                Events = testEvents
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
