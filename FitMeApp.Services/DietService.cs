using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services
{
    public class DietService : IDietService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;
        public DietService(IRepository repository)
        {
            _repository = repository;
            _mapper = new EntityModelMapper();
        }




        public int AddAnthropometricInfo(AnthropometricInfoModel infoModel)
        {
            var infoEntityBase = _mapper.MapAnthropometricInfoModelToEntityBase(infoModel);
            int infoId = _repository.AddAnthropometricInfo(infoEntityBase);
            return infoId;
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
