﻿using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Common;
using Microsoft.AspNetCore.Identity;

namespace FitMeApp.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IFitMeService _fitMeService;
        private readonly UserManager<User> _userManager;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public TrainingsController(ITrainingService trainingService, IFitMeService fitMeService,UserManager<User> userManager, ILogger<TrainersController> logger)
        {
            _trainingService = trainingService;
            _fitMeService = fitMeService;
            _userManager = userManager;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }



        public async Task<IActionResult> ApplyForPersonalTraining(string trainerId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (trainerId == user.Id || string.IsNullOrEmpty(trainerId))
            {
                return RedirectToAction("Index", "Trainers");
            }

            var trainer = _fitMeService.GetTrainerWithGymAndTrainings(trainerId);
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
        public IActionResult ApplyForPersonalTraining(ApplyingForPersonalTrainingViewModel model)
        {
            if (ModelState.IsValid)
            {
                bool userHasAvailableSubscription = _trainingService.CheckIfUserHasAvailableSubscription(model.UserId, model.SelectedDate, model.GymId);
                int trainingId = _fitMeService.GetAllTrainingModels().Where(x => x.Name == "Personal training").First().Id;  //do some Enum with trainings names
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
                    bool result = _trainingService.AddEvent(eventModel);
                    if (result)
                    {
                        return RedirectToAction("Index", "Trainers"); // do redirect to confirm page
                    }
                }
                else
                {
                    return RedirectToAction("NoAvailableSubscription", new{gymId = model.GymId});
                }
            }
            else
            {
                ModelState.AddModelError("SelectedStartTime", "Please, choose start time");
            }

            return View(model);
        }


        public IActionResult NoAvailableSubscription(int gymId)
        {
            return View("NoAvailableSubscription", gymId.ToString());
        }
    }
}