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
                AnthropometricInfoModel infoModel = AddAnthropometricInfoToDb(viewModel);
                
                if (viewModel.CurrentCalorieIntake == 0)
                {
                    viewModel.CurrentCalorieIntake = _dietService.CalculatingCurrentDailyCalories(infoModel);
                }

                DietModel dietModel = AddDietToDb(infoModel, viewModel, out bool itIsMinAllowedCaloriesValue);

                var dietViewModel = new DietViewModel()
                {
                    Id = dietModel.Id,
                    CurrentCalorieIntake = viewModel.CurrentCalorieIntake,
                    Goal = viewModel.Goal,
                    RequiredCalorieIntake = dietModel.RequiredCalorieIntake,
                    ItIsMinAllowedCaloriesValue = itIsMinAllowedCaloriesValue,
                    Proteins = dietModel.Proteins,
                    Fats = dietModel.Fats,
                    Carbohydrates = dietModel.Carbohydrates
                };

                return View("DietPlan", dietViewModel);
            }

            return View(viewModel);
        }


        public IActionResult DietPlan(DietViewModel viewModel)
        {
            return View(viewModel);
        }

        public IActionResult RefreshDietPreferences()
        {
            return ViewComponent("DietPreferences");
        }

        [HttpPost]
        public IActionResult DietPlan(DietPreferencesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var allergicTo = new List<string>();

                if (!string.IsNullOrWhiteSpace(viewModel.AllergicTo))
                {
                    string[] foodItems = viewModel.AllergicTo.Split(',');
                    foreach (var item in foodItems)
                    {
                        allergicTo.Add(item);
                    }
                }

                var dietPreferencesModel = new DietPreferencesModel()
                {
                    UserId = userId,
                    LovedNutrients = viewModel.LovedNutrients == null? new NutrientsModel() : viewModel.LovedNutrients,
                    UnlovedNutrients = viewModel.UnlovedNutrients == null? new NutrientsModel() : viewModel.UnlovedNutrients,
                    AllergicTo = allergicTo,
                    Budget = viewModel.Budget
                };

                bool success = _dietService.CreateDietPlan(dietPreferencesModel);
                if (success)
                {
                    //return view success
                }
                else
                {
                    string message =
                        "Unsuccessful attempt to create your Diet plan. " +
                        "Your anthropometric data have been saved in your Profile. " +
                        "Try to finish creating the diet plan later in your Profile, please.";
                    return View("CustomError", message);
                }
            }

            return View();
        }




        private AnthropometricInfoModel AddAnthropometricInfoToDb(AnthropometricInfoViewModel viewModel)
        {
            var userId = _userManager.GetUserId(User);

            var infoModel = new AnthropometricInfoModel()
            {
                UserId = userId,
                Gender = viewModel.Gender.ToString(),
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

            infoModel.Id = infoModelId;

            return infoModel;
        }


        private DietModel AddDietToDb(AnthropometricInfoModel infoModel, AnthropometricInfoViewModel viewModel, out bool needToAddActivity)
        {
            int requiredCalories = _dietService.CalculatingRequiredDailyCalories(infoModel, viewModel.CurrentCalorieIntake, viewModel.Goal, out bool itIsMinAllowedCaloriesValue);
            IDictionary<NutrientsEnum, int> nutrientsRates = _dietService.GetNutrientsRates(requiredCalories, viewModel.Height, viewModel.Gender, viewModel.Goal);

            var dietModel = new DietModel()
            {
                AnthropometricInfoId = infoModel.Id,
                CurrentCalorieIntake = viewModel.CurrentCalorieIntake,
                DietGoalId = (int)viewModel.Goal,
                RequiredCalorieIntake = requiredCalories,
                Proteins = nutrientsRates[NutrientsEnum.proteins],
                Fats = nutrientsRates[NutrientsEnum.fats],
                Carbohydrates = nutrientsRates[NutrientsEnum.carbohydrates]
            };

            string userId = _userManager.GetUserId(User);
            int dietId = _dietService.AddDiet(dietModel, userId);
            if (dietId == 0)
            {
                _logger.LogError("Failed attempt to add new diet info to DB");
            }

            dietModel.Id = dietId;
            needToAddActivity = itIsMinAllowedCaloriesValue;
            return dietModel;
        }





    }
}
