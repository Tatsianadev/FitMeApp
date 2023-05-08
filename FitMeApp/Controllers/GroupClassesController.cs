using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class GroupClassesController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly UserManager<User> _userManager;
        private readonly ModelViewModelMapper _mapper;
        private readonly ILogger _logger;

        public GroupClassesController(ITrainingService trainingService, IGymService gymService, ITrainerService trainerService, UserManager<User> userManager, ILogger<GroupClassesController> logger)
        {
            _trainingService = trainingService;
            _gymService = gymService;
            _trainerService = trainerService;
            _userManager = userManager;
            _mapper = new ModelViewModelMapper();
            _logger = logger;
        }

        public IActionResult GroupClasses()
        {
            var groupClassesViewModels = new List<TrainingViewModel>();
            var groupClassesModels = _trainingService.GetAllTrainingModels().Where(x => x.Name != "Personal training"); //todo enum for training name
            foreach (var groupClassModel in groupClassesModels)
            {
                groupClassesViewModels.Add(_mapper.MapTrainingModelToViewModelBase(groupClassModel));
            }

            return View(groupClassesViewModels);
        }


        public IActionResult CurrentGroupClass(int groupClassId)
        {
            var trainingModel = _trainingService.GetTrainingModel(groupClassId);
            var trainingViewModel = _mapper.MapTrainingModelToViewModel(trainingModel);
            return View(trainingViewModel);
        }
    }
}
