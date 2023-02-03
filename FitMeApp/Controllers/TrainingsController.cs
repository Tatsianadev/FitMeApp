using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<int> availableTimeInMinutes = _trainingService
                .GetAvailableToApplyTrainingTimingByTrainer(trainerId, new DateTime(2023, 02, 07))
                .ToList();
            return View();
        }
    }
}
