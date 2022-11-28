using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ScheduleController(IScheduleService scheduleService, UserManager<User> userManager, ILoggerFactory loggerFactory)
        {
            _scheduleService = scheduleService;
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger("ScheduleController");
            _mapper = new ModelViewModelMapper();
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
            ViewBag.EventsCount = _scheduleService.GetEventsCountForEachDateByUser(_userManager.GetUserId(User));
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
                Day = 0
               
            };
           
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View("Index", model);
        }


        //Events - PartialView

        [Authorize(Roles = "admin, user")]
        public IActionResult ShowEvents(int year, int month, int day)
        {
            try
            {
                DateTime currentDate = new DateTime(year, month, day);
                string userId = _userManager.GetUserId(User);
                var eventModels = _scheduleService.GetEventsByUserAndDate(userId, currentDate);

                List<EventViewModel> eventsViewModels = new List<EventViewModel>();
                foreach (var eventModel in eventModels)
                {
                    eventsViewModels.Add(_mapper.MappEventModelToViewModel(eventModel));
                }

                CurrentDayEventsViewModel model = new CurrentDayEventsViewModel()
                {
                    Year = year,
                    Month = month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month),
                    Day = day,
                    DayName = new DateTime(year, month, day).DayOfWeek.ToString(),
                    Events = eventsViewModels
                };

                ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                return View("Index", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
                   
        }

        public IActionResult Events()
        {
            return PartialView();
        }
    }
}
