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
    [Authorize(Roles = "admin, user, trainer")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly IFitMeService _fitMeService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ScheduleController(IScheduleService scheduleService, IFitMeService fitMeService, UserManager<User> userManager, ILoggerFactory loggerFactory)
        {
            _scheduleService = scheduleService;
            _fitMeService = fitMeService;
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
            if (User.IsInRole("trainer"))
            {
                ViewBag.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByTrainer(_userManager.GetUserId(User));
            }
            else
            {
                ViewBag.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByUser(_userManager.GetUserId(User));
            }            
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
            if (User.IsInRole("trainer"))
            {
                ViewBag.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByTrainer(_userManager.GetUserId(User));
            }
            else
            {
                ViewBag.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByUser(_userManager.GetUserId(User));
            }
            return View("Index", model);
        }


        //Events - PartialView

        [Authorize(Roles = "admin, user")]
        public IActionResult ShowUsersEvents(int year, int month, int day)
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
                ViewBag.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByUser(_userManager.GetUserId(User));                
                return View("Index", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }
                   
        }

        // two methods because it's need different views

        [Authorize(Roles = "trainer")]
        public IActionResult ShowTrainersEvents(int year, int month, int day)
        {
            try
            {                
                DateTime currentDate = new DateTime(year, month, day);
                string trainerId = _userManager.GetUserId(User);
                var trainerWorkHours = _fitMeService.GetWorkHoursByTrainer(trainerId);
                var workDayesOfWeek = trainerWorkHours.Select(x => x.DayName).ToList();
                if (!workDayesOfWeek.Contains(currentDate.DayOfWeek)) // если выбранный день выходной для тренера, возвращать DayOff picture вместо расписания
                {
                    //CurrentDayEventsViewModel modelForCalendar = new CurrentDayEventsViewModel()
                    //{
                    //    Year = year,
                    //    Month = month,
                    //    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month),
                    //    Day = 0,
                    //    DayName = null,
                    //    Events = null
                    //};
                    //return View("Index", modelForCalendar);
                }


                var eventModels = _scheduleService.GetEventsByTrainerAndDate(trainerId, currentDate);

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
                ViewBag.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByTrainer(trainerId);

                
                Dictionary<string, int> startWork = new Dictionary<string, int>();
                Dictionary<string, int> endWork = new Dictionary<string, int>();
                foreach (var item in trainerWorkHours)
                {
                    startWork.Add(item.DayName.ToString(), item.StartTime);
                    endWork.Add(item.DayName.ToString(), item.EndTime);
                }

                ViewBag.StartWork = startWork;
                ViewBag.EndWork = endWork;

              
                return View("Index", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public IActionResult UsersEvents()
        {
            return PartialView();
        }

        public IActionResult TrainersEvents()
        {
            return PartialView();
        }

        //public IActionResult TrainerDayOff()
        //{
        //    return PartialView();
        //}



        [Authorize(Roles = "trainer")]       
        public IActionResult ChangeEventsStatus(int eventId, int year, int month, int day)
        {
            bool result = _scheduleService.ChangeEventStatus(eventId);
            if (result)
            {
                 return ShowTrainersEvents(year, month, day);                
            }
            else
            {
                _logger.LogError("failed to change event status");
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "Failed to change event status. Please try again."
                };
                return View("CustomError", error);
            }
           
        }
    }



}
