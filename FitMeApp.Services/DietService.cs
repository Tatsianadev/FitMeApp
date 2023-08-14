using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.Services.Contracts.Models.Chart;

namespace FitMeApp.Services
{
    public class DietService : IDietService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;
        //private readonly IFileService _fileService;
        private readonly IReportService _reportService;
        private readonly IProductsService _productsService;

        public DietService(IRepository repository, IReportService reportService, IProductsService productsService)
        {
            _repository = repository;
            //_fileService = fileService;
            _reportService = reportService;
            _productsService = productsService;
            _mapper = new EntityModelMapper();
        }




        public int AddAnthropometricInfo(AnthropometricInfoModel infoModel)
        {
            var infoEntityBase = _mapper.MapAnthropometricInfoModelToEntityBase(infoModel);
            int infoId = _repository.AddAnthropometricInfo(infoEntityBase);
            return infoId;
        }


        public int AddDiet(DietModel dietModel, string userId)
        {
            var dietEntityBase = _mapper.MapDietModelToEntityBase(dietModel);
            int dietId = _repository.AddDiet(dietEntityBase, userId);
            return dietId;
        }


        public int CalculatingCurrentDailyCalories(AnthropometricInfoModel infoModel)
        {
            double activityRate = GetPhysicalActivityRate(infoModel.PhysicalActivity);
            int currentDailyCalories = (int)(MinAllowedCalories(infoModel) * activityRate);
            return currentDailyCalories;
        }


        public int CalculatingRequiredDailyCalories(AnthropometricInfoModel infoModel, int currentCalories, DietGoalsEnum goal, out bool itIsMinAllowedValue)
        {
            int minAllowedCalories = MinAllowedCalories(infoModel);
            double changeCaloriesRate = GetChangeCaloriesRate(goal);
            int neededCalories = (int)(currentCalories * changeCaloriesRate);

            if (neededCalories < minAllowedCalories)
            {
                neededCalories = minAllowedCalories;
                itIsMinAllowedValue = true;
            }
            else
            {
                itIsMinAllowedValue = false;
            }

            return neededCalories;
        }



        public IDictionary<NutrientsEnum, int> GetNutrientsRates(int calories, int height, GenderEnum gender, DietGoalsEnum goal)
        {
            int healthyWeight = CalculateHealthyWeight(height, gender);
            double proteins;
            double fats;

            if (gender == GenderEnum.male)
            {
                if (goal == DietGoalsEnum.loseWeight)
                {
                    proteins = healthyWeight * 2;
                    fats = healthyWeight * 0.7;
                }
                else if (goal == DietGoalsEnum.keepWeight)
                {
                    proteins = healthyWeight * 1.8;
                    fats = healthyWeight * 0.8;
                }
                else if (goal == DietGoalsEnum.putWeightOn)
                {
                    proteins = healthyWeight * 1.8;
                    fats = healthyWeight * 1;
                }
                else
                {
                    proteins = healthyWeight * 2.3;
                    fats = healthyWeight * 0.8;
                }
            }
            else
            {
                if (goal == DietGoalsEnum.loseWeight)
                {
                    proteins = healthyWeight * 1.8;
                    fats = healthyWeight * 0.8;
                }
                else if (goal == DietGoalsEnum.keepWeight)
                {
                    proteins = healthyWeight * 1.7;
                    fats = healthyWeight * 1;
                }
                else if (goal == DietGoalsEnum.putWeightOn)
                {
                    proteins = healthyWeight * 1.7;
                    fats = healthyWeight * 1.2;
                }
                else
                {
                    proteins = healthyWeight * 2.2;
                    fats = healthyWeight * 1.1;
                }
            }
            double carbohydrates = (calories - (proteins * 4) - (fats * 9)) / 4;

            var nutrientsRates = new Dictionary<NutrientsEnum, int>()
            {
                {NutrientsEnum.proteins,(int)proteins},
                {NutrientsEnum.fats, (int)fats},
                {NutrientsEnum.carbohydrates, (int)carbohydrates}
            };

            return nutrientsRates;
        }


        public bool CreateDietPlan(DietPreferencesModel model)
        {
            var anthropometricInfo = _repository.GetLatestAnthropometricInfo(model.UserId);
            if (anthropometricInfo != null)
            {
                var diet = _repository.GetDiet(anthropometricInfo.Id);
                if (diet != null)
                {
                    //DietPreferences, AnthropometricInfo, Diet are passed as parameters to the dietician-nutritionist additional service.
                    //for educational purposes, simply available dietary information
                    var dietReportModel = new DietPdfReportModel()
                    {
                        UserFirstName = model.UserFirstName,
                        UserLastName = model.UserLastName,
                        Gender = anthropometricInfo.Gender,
                        Height = anthropometricInfo.Height,
                        Weight = anthropometricInfo.Weight,
                        Age = anthropometricInfo.Age,
                        PhysicalActivity = anthropometricInfo.PhysicalActivity,
                        CurrentCalorieIntake = diet.CurrentCalorieIntake,
                        DietGoalId = diet.DietGoalId,
                        AnthropometricInfoDate = anthropometricInfo.Date,
                        RequiredCalorieIntake = diet.RequiredCalorieIntake,
                        Proteins = diet.Proteins,
                        Fats = diet.Fats,
                        Carbohydrates = diet.Carbohydrates,
                        Budget = model.Budget,
                        DietPlanCreatedDate = DateTime.Now
                    };

                    _reportService.CreateDietPlanPdf(dietReportModel);
                    return true;
                }
            }

            return false;
        }



        public UserAnthropometricAndDietModel GetAnthropometricAndDietModel(string userId)
        {
            var anthropometricAndDietModel = new UserAnthropometricAndDietModel()
            {
                AnthropometricInfo = new List<AnthropometricInfoModel>(),
                DietParameters = new DietModel()
            };

            var infoEntityBases = _repository.GetAllAnthropometricInfoByUser(userId).ToList();
            var dietEntityBase = _repository.GetDietByUser(userId);

            if (infoEntityBases.Count != 0)
            {
                if (infoEntityBases.Count > 19)
                {
                    infoEntityBases = infoEntityBases.Take(19).ToList();
                }

                foreach (var entityBase in infoEntityBases)
                {
                    var anthropometricInfoModel = _mapper.MapAnthropometricInfoEntityBaseToModel(entityBase);
                    anthropometricAndDietModel.AnthropometricInfo.Add(anthropometricInfoModel);
                }
            }

            if (dietEntityBase != null)
            {
                anthropometricAndDietModel.DietParameters = _mapper.MapDietEntityBaseToModel(dietEntityBase);
                
                //Get the date, that the diet plan is based on (it doesn't have to be the last date)
                DateTime dietCreationDate = infoEntityBases.FirstOrDefault(x => x.Id == dietEntityBase.AnthropometricInfoId).Date;
                anthropometricAndDietModel.DietParameters.Date = dietCreationDate;

                //Check if the required calorie intake is less than the minimum allowable intake. 
                int minAllowedCalories = MinAllowedCalories(anthropometricAndDietModel.AnthropometricInfo.FirstOrDefault(x=>x.Date == dietCreationDate));
                anthropometricAndDietModel.DietParameters.ItIsMinAllowedCaloriesValue =
                    anthropometricAndDietModel.DietParameters.RequiredCalorieIntake <= minAllowedCalories;
            }

            return anthropometricAndDietModel;
        }



        //IronPython is used
        public IEnumerable<string> GetAllProducts(string pythonFile)
        {
            var productNames = _productsService.GetAllProductNames(pythonFile);
            return productNames;
        }

        public ProductNutrientsModel GetProductInfoByName(string pythonFile, string productName)
        {
            var productNutrientsModel = _productsService.GetProductInfoByName(pythonFile, productName);
            return productNutrientsModel;
        }





        private double GetPhysicalActivityRate(int physicalActivityLevel)
        {
            var activityLevelRate = new Dictionary<int, double>()
            {
                {1, 1.2},
                {2, 1.375},
                {3, 1.55},
                {4, 1.725},
                {5, 1.9}
            };

            return activityLevelRate[physicalActivityLevel];
        }


        private int MinAllowedCalories(AnthropometricInfoModel infoModel)
        {
            int minAllowedCalories;
            if (infoModel.Gender == GenderEnum.male.ToString())
            {
                minAllowedCalories = (int)(66.47 + (13.75 * infoModel.Weight) + (5.003 * infoModel.Height) - (6.755 * infoModel.Age));
            }
            else
            {
                minAllowedCalories = (int)(655.1 + (9.563 * infoModel.Weight) + (1.850 * infoModel.Height) - (4.676 * infoModel.Age));
            }

            return minAllowedCalories;
        }


        private double GetChangeCaloriesRate(DietGoalsEnum goal)
        {
            var dietGoalRate = new Dictionary<int, double>()
            {
                {1, 0.85},
                {2, 1},
                {3, 1.15},
                {4, 1.25}
            };

            return dietGoalRate[(int)goal];
        }

        

        private int CalculateHealthyWeight(int height, GenderEnum gender)
        {
            int weightPercent = (gender == GenderEnum.male) ? 21 : 23;
            int healthyWeight = (int)(weightPercent * Math.Pow((double)height / 100, 2));
            return healthyWeight;
        }



    }
}
