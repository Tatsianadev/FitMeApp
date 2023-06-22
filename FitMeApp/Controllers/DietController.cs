using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.EMMA;
using FitMeApp.Common;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Controllers
{
    public class DietController : Controller
    {
        private readonly IDietService _dietService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public DietController(IDietService dietService, UserManager<User> userManager, ILogger<DietController> logger)
        {
            _dietService = dietService;
            _userManager = userManager;
            _logger = logger;
        }



        public IActionResult WelcomeToDietPlan()
        {
            return View();
        }


        [Authorize]
        public IActionResult AnthropometricInfo()
        {
            return View(new AnthropometricInfoViewModel());
        }


        [Authorize]
        [HttpPost]
        public IActionResult AnthropometricInfo(AnthropometricInfoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);

                var infoModel = new AnthropometricInfoModel()
                {
                    UserId = userId,
                    Gender = viewModel.Gender,
                    Height = viewModel.Height,
                    Weight = viewModel.Weight,
                    Age = viewModel.Age,
                    PhysicalActivity = viewModel.PhysicalActivity,
                    Date = DateTime.Today
                };

                int infoModelId = _dietService.AddAnthropometricInfo(infoModel);
                if (infoModelId == 0)
                {
                    _logger.LogError("Failed attempt to add new anthropometric info to DB");
                }

                if (viewModel.CurrentCalorieIntake == 0)
                {
                    viewModel.CurrentCalorieIntake = _dietService.CalculatingCurrentDailyCalories(infoModel);
                }
                
                int neededCalories = _dietService.CalculatingNeededDailyCalories(infoModel, viewModel.CurrentCalorieIntake, viewModel.Goal, out bool itIsMinAllowedCaloriesValue);
                if (itIsMinAllowedCaloriesValue)
                {
                    //return warning about add activity
                }

                
            }


            return View(viewModel);
        }


    }
}
