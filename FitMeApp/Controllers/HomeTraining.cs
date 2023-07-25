using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using IronPython.Runtime;
using Microsoft.Extensions.Logging;
using Exception = System.Exception;

namespace FitMeApp.Controllers
{
    public class HomeTraining : Controller
    {
        private readonly IHomeTrainingService _homeTrainingService;
        private readonly ILogger<HomeController> _logger;
        private readonly ModelViewModelMapper _mapper;

        public HomeTraining(IHomeTrainingService homeTrainingService, ILogger<HomeController> logger)
        {
            _homeTrainingService = homeTrainingService;
            _logger = logger;
            _mapper = new ModelViewModelMapper();

        }


        public async Task<IActionResult> HomeTrainingsList()
        {
            var homeTrainingModels = new List<HomeTrainingModel>();
            try
            {
                homeTrainingModels = (await _homeTrainingService.GetAllHomeTrainingsAsync()).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Home Trainings page does not available right now. Please, try again later.";
                return View("CustomError", message);
            }
           
            var homeTrainingViewModels = new List<HomeTrainingViewModel>();
            foreach (var model in homeTrainingModels)
            {
                homeTrainingViewModels.Add(_mapper.MapHomeTrainingModelToViewModel(model));
            }

            return View("HomeTrainingsList", homeTrainingViewModels);
        }
       
    }
}
