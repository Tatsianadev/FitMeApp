using FitMeApp.Mapper;
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
            ApplyingForPersonalTrainingViewModel model = new ApplyingForPersonalTrainingViewModel()
            {
               TrainerId = trainer.Id,
               TrainerFirstName = trainer.FirstName,
               TrainerLastName = trainer.LastName,
               GymName = trainer.Gym.Name,
               GymAddress = trainer.Gym.Address
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult ApplyForPersonalTraining(ApplyingForPersonalTrainingViewModel model)
        {
            if (ModelState.IsValid)
            {
                
            }
            else
            {
                ModelState.AddModelError("Day off", "Selected day is dayOff");
            }

            return View(model);
        }
    }
}
