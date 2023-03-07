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
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class TrainersController : Controller
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


        public IActionResult Index()
        {
            var trainerModels = _trainerService.GetAllTrainerModels();
            List<TrainerViewModel> trainers = new List<TrainerViewModel>();
            foreach (var trainerModel in trainerModels)
            {
                trainers.Add(_mapper.MapTrainerModelToViewModel(trainerModel));
            }

            ViewBag.Genders = new List<GenderEnum>()
            {
                GenderEnum.male,
                GenderEnum.female                
            };

            ViewBag.Specializations = new List<TrainerSpecializationsEnum>()
            {
                TrainerSpecializationsEnum.personal,
                TrainerSpecializationsEnum.group,
                TrainerSpecializationsEnum.universal
            };

            return View(trainers);
        }


        [HttpPost]
        public IActionResult TrainersFilter(List<GenderEnum> selectedGenders, List<TrainerSpecializationsEnum> selectedSpecializations)
        {
            if (selectedGenders.Count == 0 && selectedSpecializations.Count == 0)
            {
                return RedirectToAction("Index");
            }
            
            if (selectedGenders.Count == 0)
            {
                selectedGenders = Enum.GetValues(typeof(GenderEnum)).Cast<GenderEnum>().ToList();
            }

            if (selectedSpecializations.Count == 0)
            {
                selectedSpecializations = Enum.GetValues(typeof(TrainerSpecializationsEnum)).Cast<TrainerSpecializationsEnum>().ToList();
            }
            
            var trainerModels = _trainerService.GetTrainersByFilter(selectedGenders, selectedSpecializations);
            List<TrainerViewModel> trainerViewModels = new List<TrainerViewModel>();
            foreach (var trainerModel in trainerModels)
            {
                trainerViewModels.Add(_mapper.MapTrainerModelToViewModel(trainerModel));
            }

            ViewBag.Genders = new List<GenderEnum>()
            {
                GenderEnum.male,
                GenderEnum.female
            };

            ViewBag.Specializations = new List<TrainerSpecializationsEnum>()
            {
                TrainerSpecializationsEnum.personal,
                TrainerSpecializationsEnum.group,
                TrainerSpecializationsEnum.universal
            };

            return View("Index", trainerViewModels);
        }

        public IActionResult SelectedTrainer(string trainerId)
        {
            var trainerModel = _trainerService.GetTrainerWithGymAndTrainings(trainerId);
            TrainerViewModel trainer = _mapper.MapTrainerModelToViewModel(trainerModel);
            return View(trainer);
        }
    }
}
