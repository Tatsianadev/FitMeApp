using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly IFileService _fileService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public DietController(IDietService dietService, IFileService fileService, UserManager<User> userManager, ILogger<DietController> logger)
        {
            _dietService = dietService;
            _fileService = fileService;
            _userManager = userManager;
            _logger = logger;
        }



        public IActionResult WelcomeToDietPlan()
        {
            //return RedirectToAction("MyDietSection");
            return View();
        }


        [Authorize]
        public IActionResult AnthropometricInfo()
        {
            return View(new AnthropometricInfoViewModel());
        }


        [Authorize]
        [HttpPost]
        public IActionResult SaveAnthropometricInfo(AnthropometricInfoViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                AnthropometricInfoModel infoModel = AddAnthropometricInfoToDb(viewModel);
                if (infoModel == null)
                {
                    string message =
                        "Failed attempt to add the new anthropometric data to your Profile. Please, try again later.";
                    return View("CustomError", message);
                }

                return RedirectToAction("MyDietSection");
            }

            return View("AnthropometricInfo", viewModel);
        }



        [Authorize]
        [HttpPost]
        public IActionResult CalculateDietNutrients(AnthropometricInfoViewModel viewModel)
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

            return View("AnthropometricInfo", viewModel);
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
        public async Task<IActionResult> DietPlan(DietPreferencesViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var allergicTo = SplitText(viewModel.AllergicTo, ',');

                var dietPreferencesModel = new DietPreferencesModel()
                {
                    UserId = user.Id,
                    UserFirstName = user.FirstName,
                    UserLastName = user.LastName,
                    LovedNutrients = viewModel.LovedNutrients == null? new NutrientsModel() : viewModel.LovedNutrients,
                    UnlovedNutrients = viewModel.UnlovedNutrients == null? new NutrientsModel() : viewModel.UnlovedNutrients,
                    AllergicTo = allergicTo,
                    Budget = viewModel.Budget
                };

                bool success = _dietService.CreateDietPlan(dietPreferencesModel);
                if (success)
                {
                    string dietReportRelativePath = GetDietReportRelativePathIfExists(user.FirstName, user.LastName);
                    return View("DietPlanComplete", dietReportRelativePath);
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

        
        public IActionResult DietPlanComplete()
        {
            return View();
        }


       
        //"My Diet" section in Profile

        public async Task<IActionResult> MyDietSection()
        {
            var user = await _userManager.GetUserAsync(User);
            var model = _dietService.GetAnthropometricAndDietModel(user.Id);

            var viewModel = new UserAnthropometricAndDietViewModel()
            {
                AnthropometricInfo = new List<AnthropometricInfoViewModel>(),
                DietParameters = new DietViewModel()
            };

            foreach (var infoModel in model.AnthropometricInfo)
            {
                viewModel.AnthropometricInfo.Add(new AnthropometricInfoViewModel()
                {
                    Id = infoModel.Id,
                    UserId = infoModel.UserId,
                    Gender = (GenderEnum)Enum.Parse(typeof(GenderEnum), infoModel.Gender),
                    Height = infoModel.Height,
                    Weight = infoModel.Weight,
                    Age = infoModel.Age,
                    PhysicalActivity = infoModel.PhysicalActivity,
                    CurrentCalorieIntake = model.DietParameters.CurrentCalorieIntake,
                    Goal = (DietGoalsEnum)model.DietParameters.DietGoalId,
                    Date = infoModel.Date
                });
            }

            if (model.DietParameters.Id != 0)
            {
                viewModel.DietParameters = new DietViewModel()
                {
                    Id = model.DietParameters.Id,
                    CurrentCalorieIntake = model.DietParameters.CurrentCalorieIntake,
                    Goal = (DietGoalsEnum)model.DietParameters.DietGoalId,
                    RequiredCalorieIntake = model.DietParameters.RequiredCalorieIntake,
                    ItIsMinAllowedCaloriesValue = model.DietParameters.ItIsMinAllowedCaloriesValue,
                    Proteins = model.DietParameters.Proteins,
                    Fats = model.DietParameters.Fats,
                    Carbohydrates = model.DietParameters.Carbohydrates,
                    Date = model.DietParameters.Date
                };
                viewModel.DietReportRelativePath = GetDietReportRelativePathIfExists(user.FirstName, user.LastName);
            }
            
            return View(viewModel);
        }



        public IActionResult ProductsInfo()
        {
            string pythonFile = Environment.CurrentDirectory + Resources.Resources.DietJournalPyPath;
            var productNames = _dietService.GetAllProducts(pythonFile);
            if (productNames == null)
            {
                string message = "Diet journal doesn't work right now. Please, try again later.";
                return View("CustomError", message);
            }

            return View(productNames);
        }

        public IActionResult InvokeProductNutrientsViewComponent(string productName)
        {
            return ViewComponent("ProductNutrients", new {productName = productName });
        }






        private List<string> SplitText(string text, char signToSplit)
        {
            var words = new List<string>();

            if (!string.IsNullOrWhiteSpace(text))
            {
                string[] foodItems = text.ToLower().Split(signToSplit);
                char[] allDigits = Enumerable.Range('a', 'z' - 'a' + 1).Select(x => (char)x).ToArray();
                foreach (var item in foodItems)
                {
                    if (string.IsNullOrWhiteSpace(item))
                    {
                        continue;
                    }

                    if (item.StartsWith(' '))
                    {
                        int indexFirstDigit = item.IndexOfAny(allDigits);
                        string updatedItem = item.Remove(0, indexFirstDigit);
                        words.Add(updatedItem);
                    }
                    else
                    {
                        words.Add(item);
                    }
                }
            }

            return words;
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


        private string GetDietReportRelativePathIfExists(string userFirstName, string userLastName)
        {
            string dietReportRelativePath = @"/PDF/Diet/DietPlan_" + userFirstName + "_" + userLastName + ".pdf";
            string dietReportAbsolutePath = Environment.CurrentDirectory + @"\wwwroot\PDF\Diet\DietPlan_" + userFirstName + "_" + userLastName + ".pdf";
            FileInfo file = new FileInfo(dietReportAbsolutePath);
            if (file.Exists)
            {
                return dietReportRelativePath;
            }

            return string.Empty;
        }





    }
}
