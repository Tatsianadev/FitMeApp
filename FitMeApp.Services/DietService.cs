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


        public int CalculatingNeededDailyCalories(AnthropometricInfoModel infoModel, int currentCalories, DietGoalsEnum goal, out bool itIsMinAllowedValue)
        {
            //calc min allowed calorie
            int minAllowedCalories = MinAllowedCalories(infoModel);
            //calc needed for goal
            double changeCaloriesRate = GetChangeCaloriesRate(goal);
            int neededCalories = (int)(currentCalories * changeCaloriesRate);
            //compare with min
            //set flag
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

    }
}
