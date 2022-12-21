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
        private readonly IFitMeService _fitMeService;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public TrainersController(IFitMeService fitMeService, ILoggerFactory loggerFactory)
        {
            _fitMeService = fitMeService;
            _mapper = new ModelViewModelMapper();
            _logger = loggerFactory.CreateLogger("TrainersController");
        }


        public IActionResult Index()
        {
            var trainerModels = _fitMeService.GetAllTrainerModels();
            List<TrainerViewModel> trainers = new List<TrainerViewModel>();
            foreach (var trainerModel in trainerModels)
            {
                trainers.Add(_mapper.MappTrainerModelToViewModel(trainerModel));
            }

            ViewBag.Genders = new List<GenderEnum>()
            {
                GenderEnum.man,
                GenderEnum.woman                
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
            if (selectedGenders == null && selectedGenders == null)
            {
                return RedirectToAction("Index");
            }

            var trainerModels = _fitMeService.GetTrainersByFilter(selectedGenders, selectedSpecializations);
            return View();
        }
    }
}
