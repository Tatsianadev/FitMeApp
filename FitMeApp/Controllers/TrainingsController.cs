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
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly IScheduleService _scheduleService;
        private readonly UserManager<User> _userManager;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public TrainingsController(ITrainingService trainingService, 
            IGymService gymService, 
            ITrainerService trainerService, 
            IScheduleService scheduleService,
            UserManager<User> userManager, 
            ILogger<TrainersController> logger)
        {
            _trainingService = trainingService;
            _gymService = gymService;
            _trainerService = trainerService;
            _scheduleService = scheduleService;
            _userManager = userManager;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
        }



        [HttpGet]
        public async Task<IActionResult> ApplyForPersonalTraining(string trainerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (trainerId == user.Id || string.IsNullOrEmpty(trainerId))
            {
                return RedirectToAction("Index", "Trainers");
            }

            var trainer = _trainerService.GetTrainerWithGymAndTrainings(trainerId);
            ApplyingForPersonalTrainingViewModel model = new ApplyingForPersonalTrainingViewModel()
            {
               TrainerId = trainer.Id,
               TrainerFirstName = trainer.FirstName,
               TrainerLastName = trainer.LastName,
               GymId = trainer.Gym.Id,
               GymName = trainer.Gym.Name,
               GymAddress = trainer.Gym.Address,
               UserId = user.Id
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
                    bool userHasAvailableSubscription = _trainingService.CheckIfUserHasAvailableSubscription(model.UserId, model.SelectedDate, model.GymId);
                    int trainingId = _trainingService.GetAllTrainingModels().Where(x => x.Name == "Personal training").First().Id;  //todo some Enum with trainings names
                    if (userHasAvailableSubscription)
                    {
                        int starTime = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(model.SelectedStartTime);

                        EventViewModel newEvent = new EventViewModel()
                        {
                            Date = model.SelectedDate,
                            StartTime = starTime,
                            EndTime = starTime + model.DurationInMinutes,
                            TrainerId = model.TrainerId,
                            UserId = model.UserId,
                            TrainingId = trainingId,
                            Status = Common.EventStatusEnum.Open
                        };

                        var eventModel = _mapper.MapEventViewModelToModel(newEvent);
                        int eventId = _scheduleService.AddEvent(eventModel);
                        if (eventId != 0)
                        {
                            return RedirectToAction("ApplyForTrainingSubmitted", new{ isPersonalTraining = true});
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
