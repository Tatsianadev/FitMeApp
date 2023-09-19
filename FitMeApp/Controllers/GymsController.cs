using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FitMeApp.Controllers
{
    public sealed class GymsController : Controller
    {
        private readonly IGymService _gymService;
        private readonly ITrainingService _trainingService;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public GymsController(IGymService gymService, ITrainingService trainingService, ILogger<GymsController> logger)
        {
            _gymService = gymService;
            _trainingService = trainingService;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
        }


        public IActionResult AllGyms()
        {
            try
            {
                var gymModels = _gymService.GetAllGymsWithGalleryModels();
                List<GymViewModel> gyms = new List<GymViewModel>();
                foreach (var gym in gymModels)
                {
                    gyms.Add(_mapper.MapGymModelToViewModelBase(gym));
                }

                var trainingModels = _trainingService.GetAllTrainingModels(); //info for filter by trainings
                List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                foreach (var training in trainingModels)
                {
                    trainings.Add(_mapper.MapTrainingModelToViewModelBase(training));
                }
                ViewBag.Trainings = trainings;

                return View(gyms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Gym page is not available now.";
                return View("CustomError", message);
            }
        }


        [HttpPost]
        public IActionResult GymsByFilter(List<int> selectedTrainingsId)
        {
            try
            {
                if (selectedTrainingsId.Count == 0)
                {
                    return RedirectToAction("AllGyms");
                }

                var selectedGymModels = _gymService.GetGymsByTrainings(selectedTrainingsId);
                List<GymViewModel> selectedGyms = new List<GymViewModel>();
                foreach (var selectedGymModel in selectedGymModels)
                {
                    selectedGyms.Add(_mapper.MapGymModelToViewModelBase(selectedGymModel));
                }

                var trainingModels = _trainingService.GetAllTrainingModels(); //info for filter by trainings
                List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                foreach (var training in trainingModels)
                {
                    trainings.Add(_mapper.MapTrainingModelToViewModelBase(training));
                }
                ViewBag.Trainings = trainings;

                return View("AllGyms", selectedGyms);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with using filters. Please, try again later.";
                return View("CustomError", message);
            }
        }


        public IActionResult CurrentGymInfo(int gymId)
        {
            try
            {
                var gymModel = _gymService.GetGymModel(gymId);
                GymViewModel gym = _mapper.MapGymModelToViewModel(gymModel);
                List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                foreach (var trainer in gym.Trainers)
                {
                    foreach (var training in trainer.Trainings)
                    {
                        if (!trainings.Select(x => x.Id).ToList().Contains(training.Id) &&
                            training.Name != Common.TrainingsEnum.personaltraining.GetDescription())
                        {
                            trainings.Add(training);
                        }
                    }
                }

                ViewBag.Trainings = trainings;
                ViewBag.WorkHours = _gymService.GetWorkHoursByGym(gymId);
                return View(gym);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Gym page is not available now.";
                return View("CustomError", message);
            }
        }
    }
}
