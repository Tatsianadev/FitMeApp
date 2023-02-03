using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Common;

namespace FitMeApp.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IFitMeService _fitMeService;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public TrainingsController(ITrainingService trainingService, IFitMeService fitMeService, ILogger<TrainersController> logger)
        {
            _trainingService = trainingService;
            _fitMeService = fitMeService;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ApplyForPersonalTraining(string trainerId)
        {
            var trainer = _fitMeService.GetTrainerWithGymAndTrainings(trainerId);
            List<int> availableTimeInMinutes = _trainingService
                .GetAvailableToApplyTrainingTimingByTrainer(trainerId, DateTime.Now)
                .ToList();

            List<string> stringTime = new List<string>();
            foreach (var intTime in availableTimeInMinutes)
            {
                stringTime.Add(WorkHoursTypesConverter.ConvertIntTimeToString(intTime));
            }

            ApplyingForPersonalTrainingViewModel model = new ApplyingForPersonalTrainingViewModel()
            {
               TrainerId = trainer.Id,
               TrainerFirstName = trainer.FirstName,
               TrainerLastName = trainer.LastName,
               GymName = trainer.Gym.Name,
               GymAddress = trainer.Gym.Address,
               AvailableTime = stringTime,
               SelectedDate = DateTime.Now

            };

            return View(model);
        }
    }
}
