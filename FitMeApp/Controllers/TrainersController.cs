using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Filters;


namespace FitMeApp.Controllers
{
    public sealed class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public TrainersController(ITrainerService trainerService, ILogger<TrainersController> logger)
        {
            _trainerService = trainerService;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
        }

        [TypeFilter(typeof(MethodCallCounterAttribute))]
        public IActionResult AllTrainers()
        {
            try
            {
                var trainerModels = _trainerService.GetAllTrainerModels();
                List<TrainerViewModel> trainers = new List<TrainerViewModel>();
                foreach (var trainerModel in trainerModels)
                {
                    trainers.Add(_mapper.MapTrainerModelToViewModel(trainerModel));
                }

                return View(trainers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "The list of trainers is not available now. Please, try again later.";
                return View("CustomError", message);
            }
           
        }


        [HttpPost]
        public IActionResult TrainersFilter(List<GenderEnum> selectedGenders, List<TrainerSpecializationsEnum> selectedSpecializations)
        {
            if (selectedGenders.Count == 0 && selectedSpecializations.Count == 0)
            {
                return RedirectToAction("AllTrainers");
            }
            
            if (selectedGenders.Count == 0)
            {
                selectedGenders = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().ToList();
            }

            if (selectedSpecializations.Count == 0)
            {
                selectedSpecializations = Enum.GetValues(typeof(TrainerSpecializationsEnum)).Cast<TrainerSpecializationsEnum>().ToList();
            }

            try
            {
                var trainerModels = _trainerService.GetTrainersByFilter(selectedGenders, selectedSpecializations);
                List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
                foreach (var trainerModel in trainerModels)
                {
                    trainerViewModels.Add(_mapper.MapTrainerModelToViewModel(trainerModel));
                }

                return View("AllTrainers", trainerViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Filter is not working right now. Please, try again later.";
                return View("CustomError", message);
            }

        }


        public IActionResult SelectedTrainer(string trainerId)
        {
            try
            {
                var trainerModel = _trainerService.GetTrainerWithGymAndTrainings(trainerId);
                TrainerViewModel trainer = _mapper.MapTrainerModelToViewModel(trainerModel);
                return View(trainer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "The selected trainer page is not available now. Please, try again later.";
                return View("CustomError", message);
            }
        }
    }
}
