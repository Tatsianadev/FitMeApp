using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Controllers;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FitMeApp.ViewComponents
{
    public class HomeTrainingsListViewComponent: ViewComponent
    {
        private readonly IHomeTrainingService _homeTrainingService;
        private readonly ILogger<HomeController> _logger;
        private readonly ModelViewModelMapper _mapper;

        public HomeTrainingsListViewComponent(IHomeTrainingService homeTrainingService, ILogger<HomeController> logger)
        {
            _homeTrainingService = homeTrainingService;
            _logger = logger;
            _mapper = new ModelViewModelMapper();

        }

        public async Task<IViewComponentResult> InvokeAsync(string gender = "", int age = 0, int calorie = 0, int duration = 0,
            bool equipment = true)
        {
            var homeTrainingModels = new List<HomeTrainingModel>();
           
            try
            {
                if (gender == string.Empty && age == 0 && calorie == 0 && duration == 0 && equipment == true)
                {
                    homeTrainingModels = (await _homeTrainingService.GetAllHomeTrainingsAsync()).ToList();
                }
                else
                {
                    homeTrainingModels = (await _homeTrainingService.GetHomeTrainingsByFilterAsync(gender, age, calorie, duration, equipment)).ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Home Trainings page does not available right now. Please, try again later.";
                return await Task.FromResult(View("CustomError", message));
            }

            var homeTrainingViewModels = new List<HomeTrainingViewModel>();
            foreach (var model in homeTrainingModels)
            {
                homeTrainingViewModels.Add(_mapper.MapHomeTrainingModelToViewModel(model));
            }

            return await Task.FromResult(View("HomeTrainingsList", homeTrainingViewModels));

        }

    }
}
