using System;
using System.Collections.Generic;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FitMeApp.Controllers
{
    public sealed class TrainingsController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly ITrainerService _trainerService;
        private readonly IScheduleService _scheduleService;
        private readonly UserManager<User> _userManager;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public TrainingsController(ITrainingService trainingService,
            ITrainerService trainerService,
            IScheduleService scheduleService,
            UserManager<User> userManager,
            ILogger<TrainersController> logger)
        {
            _trainingService = trainingService;
            _trainerService = trainerService;
            _scheduleService = scheduleService;
            _userManager = userManager;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
        }



        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ApplyForPersonalTraining(string trainerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (trainerId == user.Id || string.IsNullOrEmpty(trainerId))
            {
                return RedirectToAction("AllTrainers", "Trainers");
            }

            var trainer = _trainerService.GetTrainerWithGymAndTrainings(trainerId);
            int price = _trainingService.GetPrice(trainerId);
            ApplyingForPersonalTrainingViewModel model = new ApplyingForPersonalTrainingViewModel()
            {
                TrainerId = trainer.Id,
                TrainerFirstName = trainer.FirstName,
                TrainerLastName = trainer.LastName,
                GymId = trainer.Gym.Id,
                GymName = trainer.Gym.Name,
                GymAddress = trainer.Gym.Address,
                SelectedDate = DateTime.Today,
                UserId = user.Id,
                Price = price
            };

            return View(model);
        }



        [HttpPost]
        [Authorize]
        public IActionResult ApplyForPersonalTraining(ApplyingForPersonalTrainingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int starTime = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(model.SelectedStartTime);
                    int endTime = starTime + model.DurationInMinutes;

                    bool userHasAvailableSubscription = _trainingService.CheckIfUserHasAvailableSubscription(model.UserId, model.SelectedDate, model.GymId);
                    bool noEventsAtSelectedTime = _scheduleService.CheckIfNoEventsAtSelectedTime(model.UserId, starTime, endTime, model.SelectedDate);

                    //if current user is group or universal Trainer
                    //and there are not events at the selected time (otherwise -> no point to check group schedule)
                    if (User.IsInRole(RolesEnum.trainer.ToString()) &&
                        _trainerService.GetTrainerSpecialization(model.UserId) != TrainerSpecializationsEnum.personal.ToString() &&
                        noEventsAtSelectedTime)
                    {
                        noEventsAtSelectedTime = _scheduleService.CheckIfNoGroupClassesAtSelectedTime(model.UserId, starTime, endTime, model.SelectedDate);
                    }

                    if (noEventsAtSelectedTime == false)
                    {
                        ModelState.AddModelError("SelectedStartTime", "You have already had an appointment at selected time. Please, choose another time.");
                        return View(model);
                    }

                    int trainingId = (int)TrainingsEnum.personaltraining;
                    if (userHasAvailableSubscription)
                    {

                        EventViewModel newEvent = new EventViewModel()
                        {
                            Date = model.SelectedDate,
                            StartTime = starTime,
                            EndTime = endTime,
                            TrainerId = model.TrainerId,
                            UserId = model.UserId,
                            TrainingId = trainingId,
                            Status = Common.EventStatusEnum.Open
                        };

                        var eventModel = _mapper.MapEventViewModelToModel(newEvent);
                        int eventId = _scheduleService.AddEvent(eventModel);
                        if (eventId != 0)
                        {
                            return RedirectToAction("ApplyForTrainingSubmitted", new { isPersonalTraining = true });
                        }
                    }
                    else
                    {
                        return RedirectToAction("NoAvailableSubscription", new { gymId = model.GymId });
                    }
                }
                else
                {
                    ModelState.AddModelError("SelectedStartTime", "Please, choose start time");
                }

                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Failed to apply for personal training. Please, try again later.";
                return View("CustomError", message);
            }

        }


        public IActionResult NoAvailableSubscription(int gymId)
        {
            return View("NoAvailableSubscription", gymId.ToString());
        }

        public IActionResult ApplyForTrainingSubmitted(bool isPersonalTraining)
        {
            return View("ApplyForTrainingSubmitted", isPersonalTraining);
        }
    }
}
