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
            if (User.IsInRole("trainer"))
            {
                var trainerWorkHours = _fitMeService.GetWorkHoursByTrainer(_userManager.GetUserId(User));
                if (trainerWorkHours.Count() == 0)
                {
                    return View("NoTrainerWorkHours");
                }
            }

            int month = DateTime.Today.Month;
            int year = DateTime.Today.Year;

            CalendarPageWithEventsViewModel model = new CalendarPageWithEventsViewModel()
            {
                Date = new DateTime(year, month, 1),
                MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month),
                DayOnCalendarSelected = false
            };

            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            if (User.IsInRole("trainer"))
            {
                model.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByTrainer(_userManager.GetUserId(User));
            }
            else
            {
                model.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByUser(_userManager.GetUserId(User));
            }
            return View(model);
        }




        //Caledar - PartialView

        public IActionResult Calendar()
        {
            return PartialView();
        }

        [HttpPost]
        public IActionResult CalendarCarousel(CalendarPageWithEventsViewModel model)
        {
            model.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.Date.Month);
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;

            return View("Index", model);
        }


        //Events - PartialView

        [Authorize(Roles = "admin, user")]
        public IActionResult ShowUsersEvents(CalendarPageWithEventsViewModel model)
        {
            try
            {
                string userId = _userManager.GetUserId(User);

                var eventModels = _scheduleService.GetEventsByUserAndDate(userId, model.Date);
                List<EventViewModel> eventsViewModels = new List<EventViewModel>();
                foreach (var eventModel in eventModels)
                {
                    eventsViewModels.Add(_mapper.MappEventModelToViewModel(eventModel));
                }

                model.Events = eventsViewModels;
                model.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.Date.Month);
                model.SelectedDayIsWorkOff = false;

                ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;

                return View("Index", model);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // two methods because it's need different views

        [Authorize(Roles = "trainer")]
        [HttpPost]
        public IActionResult ShowTrainersEvents(CalendarPageWithEventsViewModel model)
        {
            try
            {
                ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                string trainerId = _userManager.GetUserId(User);
                var trainerWorkHours = _fitMeService.GetWorkHoursByTrainer(trainerId);
                var workDayesOfWeek = trainerWorkHours.Select(x => x.DayName).ToList();

                if (!workDayesOfWeek.Contains(model.Date.DayOfWeek)) // если выбранный день выходной для тренера, открывать WorkOffPartialView
                {
                    model.SelectedDayIsWorkOff = true;
                    return View("Index", model);
                }

                var eventModels = _scheduleService.GetEventsByTrainerAndDate(trainerId, model.Date);
                List<EventViewModel> eventsViewModels = new List<EventViewModel>();
                foreach (var eventModel in eventModels)
                {
                    eventsViewModels.Add(_mapper.MappEventModelToViewModel(eventModel));
                }

                model.Events = eventsViewModels;
                model.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.Date.Month);
                model.SelectedDayIsWorkOff = false;

                int startWork = trainerWorkHours.Where(x => x.DayName == model.Date.DayOfWeek).Select(x => x.StartTime).First();
                int endWork = trainerWorkHours.Where(x => x.DayName == model.Date.DayOfWeek).Select(x => x.EndTime).First();
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

        public IActionResult TrainerDayOff()
        {
            return PartialView();
        }

        public IActionResult NoTrainerWorkHours()
        {
            return View();
        }



        [Authorize(Roles = "trainer")]
        public IActionResult ChangeEventsStatus(int eventId, CalendarPageWithEventsViewModel model)
        {
            bool result = _scheduleService.ChangeEventStatus(eventId);
            if (result)
            {
                return ShowTrainersEvents(model);
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
