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
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using FitMeApp.Services.Contracts.Models;
using Quartz.Xml.JobSchedulingData20;
using Task = DocumentFormat.OpenXml.Office2021.DocumentTasks.Task;

namespace FitMeApp.Controllers
{
    [Authorize(Roles = "admin, user, trainer")]
    public sealed class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ITrainerService _trainerService;
        private readonly ITrainingService _trainingService;
        private readonly IEmailService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;

        public ScheduleController(IScheduleService scheduleService,
            ITrainerService trainerService,
            ITrainingService trainingService,
            IEmailService emailService,
            UserManager<User> userManager,
            ILogger<ScheduleController> logger)
        {
            _scheduleService = scheduleService;
            _trainerService = trainerService;
            _trainingService = trainingService;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
            _mapper = new ModelViewModelMapper();
        }



        public IActionResult Index()
        {
            if (User.IsInRole("trainer"))
            {
                var trainerWorkHours = _trainerService.GetWorkHoursByTrainer(_userManager.GetUserId(User));
                if (!trainerWorkHours.Any())
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
            model.DatesEventsCount = _scheduleService.GetEventsCountForEachDateByUser(_userManager.GetUserId(User));
            
            if (User.IsInRole("trainer"))
            {
                IDictionary<DateTime,int> trainersDatesEventsCount = _scheduleService.GetEventsCountForEachDateByTrainer(_userManager.GetUserId(User));
                foreach (var dateEventElement in trainersDatesEventsCount)
                {
                    if (model.DatesEventsCount.ContainsKey(dateEventElement.Key))
                    {
                        model.DatesEventsCount[dateEventElement.Key] = model.DatesEventsCount[dateEventElement.Key] + dateEventElement.Value;
                    }
                    else
                    {
                        model.DatesEventsCount.Add(dateEventElement);
                    }
                }
            }

            return View(model);
        }




        //Calendar - PartialView

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
                    eventsViewModels.Add(_mapper.MapEventModelToViewModel(eventModel));
                }

                model.Events = eventsViewModels;
                model.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.Date.Month);
                model.SelectedDayIsWorkOff = false;

                ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;

                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with display events. Please, try again later.";
                return View("CustomError", message);
            }

        }

        // two methods because it's need different views

        [Authorize(Roles = "trainer")]
        [HttpPost]
        public async Task<IActionResult> ShowTrainersEvents(CalendarPageWithEventsViewModel model) //todo separate this method for a few smaller ones
        {
            try
            {
                ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
                var trainer = await _userManager.GetUserAsync(User);
                var trainerWorkHours = _trainerService.GetWorkHoursByTrainer(trainer.Id).ToList();
                var workDaysOfWeek = trainerWorkHours.Select(x => x.DayName).ToList();
                var trainerSpecialization = _trainerService.GetTrainerSpecialization(trainer.Id);
                int startWork = trainerWorkHours.Where(x => x.DayName == model.Date.DayOfWeek).Select(x => x.StartTime).First();
                int endWork = trainerWorkHours.Where(x => x.DayName == model.Date.DayOfWeek).Select(x => x.EndTime).First();
                
                List<EventViewModel> eventsViewModels = new List<EventViewModel>();

                var trainingsAsCustomer = _scheduleService.GetEventsByUserAndDate(trainer.Id, model.Date).ToList();

                // If selected day is WorkOffDay for trainer -> display WorkOffPartialView
                if (!workDaysOfWeek.Contains(model.Date.DayOfWeek) && !trainingsAsCustomer.Any())
                {
                    model.SelectedDayIsWorkOff = true;
                    return View("Index", model);
                }

                // If there are some trainings where current trainer is customer -> add them to schedule
                if (trainingsAsCustomer.Count() != 0)
                {
                    foreach (var trainingEvent in trainingsAsCustomer)
                    {
                        eventsViewModels.Add(_mapper.MapEventModelToViewModel(trainingEvent));
                    }

                    //if time of trainings, where current trainer is customer, are outside the trainer work hours ->
                    //change the schedule time scale to display this trainings.
                    int earlierTrainingAsCustomerStartTime = trainingsAsCustomer.Where(x => x.Date == model.Date).Select(x => x.StartTime).Min();
                    int latestTrainingAsCustomerEndTime = trainingsAsCustomer.Where(x => x.Date == model.Date).Select(x => x.EndTime).Max();

                    if (earlierTrainingAsCustomerStartTime < startWork)
                    {
                        startWork = earlierTrainingAsCustomerStartTime;
                    }

                    if (latestTrainingAsCustomerEndTime > endWork)
                    {
                        endWork = latestTrainingAsCustomerEndTime;
                    }
                }

                //Personal trainings and group classes are shown different
                // If trainer do personal trainings -> Get all personal trainings for selected date
                if (trainerSpecialization == TrainerSpecializationsEnum.universal.ToString() ||
                    trainerSpecialization == TrainerSpecializationsEnum.personal.ToString())
                {
                    eventsViewModels.AddRange(GetPersonalTrainingsPerDateByTrainer(trainer.Id, model.Date));
                }

                //If trainer do group classes ->  Get all group classes for selected date
                if (trainerSpecialization == TrainerSpecializationsEnum.universal.ToString() ||
                    trainerSpecialization == TrainerSpecializationsEnum.group.ToString())
                {
                    eventsViewModels.AddRange(GetGroupClassesPerDateByTrainer(trainer.Id, model.Date));
                }

                model.Events = eventsViewModels.OrderBy(x => x.StartTime).ToList();
                model.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(model.Date.Month);
                model.SelectedDayIsWorkOff = false;

                ViewBag.StartWork = startWork;
                ViewBag.EndWork = endWork;

                return View("Index", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with display events. Please, try again later.";
                return View("CustomError", message);
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
        public async Task<IActionResult> ConfirmEvent(int eventId, CalendarPageWithEventsViewModel model)
        {
            _scheduleService.ConfirmEvent(eventId);
            return await ShowTrainersEvents(model);
        }


        [Authorize(Roles = "trainer")]
        public async Task<IActionResult> DeletePersonalTrainingEvent(int eventId, CalendarPageWithEventsViewModel model)
        {
            var eventModel = _scheduleService.GetEvent(eventId);
            if (eventModel != null)
            {
                var user = await _userManager.FindByIdAsync(eventModel.UserId);
                if (user != null)
                {
                    string userName = user.FirstName;
                    SendPersonalTrainingCanceledEmail(eventModel, userName);
                }

                _scheduleService.DeleteEvent(eventId);
            }

            return await ShowTrainersEvents(model);
        }


        public async Task<IActionResult> DeleteGroupClassScheduleRecord(int grClassScheduleRecordId, int actualParticipantsCount) //todo finish this method (by form)
        {
            if (actualParticipantsCount != 0)
            {
                var groupClassFullInfo = _trainingService.GetRecordInGroupClassSchedule(grClassScheduleRecordId);
                if (groupClassFullInfo == null)
                {
                    string message = "Failed attempt to cancel group class. Please, try again later.";
                    return View("CustomError", message);
                }
                var participantIds = _trainingService.GetAllParticipantIdsByGroupClass(grClassScheduleRecordId);
                var participants = _userManager.Users.Where(x => participantIds.Contains(x.Id)).ToList();

                //send the emails to participants
                foreach (var user in participants)
                {
                    var callbackUrl = Url.Action(
                        "Index",
                        "Home",
                        new { },
                        protocol: HttpContext.Request.Scheme);

                    string grClassDate = groupClassFullInfo.Date.Date.ToString("MM/dd/yyyy");
                    string startTime = Common.WorkHoursTypesConverter.ConvertIntTimeToString(groupClassFullInfo.StartTime);

                    string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study case - constant
                    string fromEmail = DefaultSettingsStorage.SenderEmail;
                    string plainTextContent = $"We have schedule changes!" +
                                              $" {groupClassFullInfo.GroupClassName} class {grClassDate} at {startTime} has been canceled." +
                                              $"For more information follow the link <a href=\"" + callbackUrl + "\">FitMe</a>";
                    string htmlContent = $"<strong> We have schedule changes!" +
                                         $" {groupClassFullInfo.GroupClassName} class {grClassDate} at {startTime} has been canceled." +
                                         $"For more information follow the link <a href=\"" + callbackUrl + $"\">{callbackUrl}</a></strong>";
                    string subject = $"{groupClassFullInfo.GroupClassName} canceled";

                    await _emailService.SendEmailAsync(toEmail, user.FirstName, fromEmail, subject, plainTextContent, htmlContent);
                }
            }

            _trainingService.DeleteGroupClassScheduleRecord(grClassScheduleRecordId, actualParticipantsCount);

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CancelEventAsCustomer(int eventId, CalendarPageWithEventsViewModel model)
        {
            try
            {
                var eventModel = _scheduleService.GetEvent(eventId);
                var trainer = _userManager.Users.FirstOrDefault(x => x.Id == eventModel.TrainerId);
                var customer = _userManager.Users.FirstOrDefault(x => x.Id == eventModel.UserId);

                if (trainer == null || customer == null)
                {
                    string message = "There are some problems with cancel the training. Please, try again later.";
                    return View("CustomError", message);
                }

                if (eventModel.TrainingId == (int)TrainingsEnum.personaltraining)
                {
                    string trainerName = trainer.FirstName;
                    SendPersonalTrainingCanceledEmail(eventModel, trainerName);
                }
                else
                {
                    int groupClassScheduleRecordId = _trainingService.GetGroupClassScheduleRecordId(trainer.Id, eventModel.TrainingId, eventModel.Date, eventModel.StartTime);
                    _trainingService.DeleteParticipant(customer.Id, groupClassScheduleRecordId);
                }

                _scheduleService.DeleteEvent(eventId);

                if (User.IsInRole("trainer"))
                {
                    return await ShowTrainersEvents(model);
                }
                else
                {
                    return ShowUsersEvents(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There are some problems with cancel the training. Please, try again later.";
                return View("CustomError", message);
            }
        }





        private IEnumerable<EventViewModel> GetPersonalTrainingsPerDateByTrainer(string trainerId, DateTime date)
        {
            var personalTrainingsPerDateByTrainer = new List<EventViewModel>();
            var personalTrainings = _scheduleService.GetPersonalTrainingsByTrainerAndDate(trainerId, date);
            foreach (var personalTraining in personalTrainings)
            {
                personalTrainingsPerDateByTrainer.Add(_mapper.MapEventModelToViewModel(personalTraining));
            }

            return personalTrainingsPerDateByTrainer;
        }


        private IEnumerable<EventViewModel> GetGroupClassesPerDateByTrainer(string trainerId, DateTime date)
        {
            var groupClassPerDateByTrainer = new List<EventViewModel>();
            var groupClassEventModels = _trainingService.GetAllRecordsInGroupClassScheduleByTrainerAndDate(trainerId, date);

            foreach (var grClassEventModel in groupClassEventModels)
            {
                groupClassPerDateByTrainer.Add(new EventViewModel()
                {
                    GroupClassScheduleRecordId = grClassEventModel.Id,
                    TrainerId = grClassEventModel.TrainerId,
                    TrainingId = grClassEventModel.GroupClassId,
                    TrainingName = grClassEventModel.GroupClassName,
                    Date = grClassEventModel.Date,
                    StartTime = grClassEventModel.StartTime,
                    EndTime = grClassEventModel.EndTime,
                    ParticipantsLimit = grClassEventModel.ParticipantsLimit,
                    ActualParticipantsCount = grClassEventModel.ActualParticipantsCount,
                    Status = EventStatusEnum.Confirmed
                });
            }

            return groupClassPerDateByTrainer;
        }


        private async void SendPersonalTrainingCanceledEmail(EventModel eventModel, string toName)
        {
            string eventDate = eventModel.Date.Date.ToString("MM/dd/yyyy");
            string startTime = Common.WorkHoursTypesConverter.ConvertIntTimeToString(eventModel.StartTime);

            string toEmail = DefaultSettingsStorage.ReceiverEmail; //should be user.Email, but for study case - constant
            string fromEmail = DefaultSettingsStorage.SenderEmail;
            string plainTextContent = $"We have schedule changes! Personal training class {eventDate} at {startTime} has been canceled.";
            string htmlContent = $"<strong> We have schedule changes! Personal training class {eventDate} at {startTime} has been canceled.</strong>";
            string subject = "Personal training canceled";

            await _emailService.SendEmailAsync(toEmail, toName, fromEmail, subject, plainTextContent, htmlContent);
        }


    }



}
