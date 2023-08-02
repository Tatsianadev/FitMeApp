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
    public class HomeTrainingController : Controller
    {
        private readonly IHomeTrainingService _homeTrainingService;
        private readonly ILogger<HomeController> _logger;
        private readonly ModelViewModelMapper _mapper;

        public HomeTrainingController(IHomeTrainingService homeTrainingService, ILogger<HomeController> logger)
        {
            _homeTrainingService = homeTrainingService;
            _logger = logger;
            _mapper = new ModelViewModelMapper();

        }


        public IActionResult WelcomeToHomeTrainings()
        {
            return View("WelcomeToHomeTrainings");
        }


        public IActionResult InvokeHomeTrainingsListViewComponent(string gender, int age, int calorie, int duration,
            bool equipment)
        {
            return ViewComponent("HomeTrainingsList", new {gender = gender, age = age, calorie = calorie, duration = duration, equipment = equipment});
        }

        public async Task<IActionResult> DownloadHomeTrainingPlan(int homeTrainingPlanId, string homeTrainingName)
        {
            string contentType = "application/pdf";
            string fileName = homeTrainingName + ".pdf";
            try
            {
                byte[] contentPdf = await _homeTrainingService.DownloadPdfFileAsync(homeTrainingPlanId);
                return File(contentPdf, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Failed to download the file. Please, try again later.";
                return View("CustomError", message);
            }
           
        }
    }
}
