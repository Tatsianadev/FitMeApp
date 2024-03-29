﻿using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.Services.Contracts.Models.Chart;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IDietService
    {
        int AddAnthropometricInfo(AnthropometricInfoModel infoModel);
        int AddDiet(DietModel dietModel, string userId);
        int CalculatingCurrentDailyCalories(AnthropometricInfoModel infoModel);
        int CalculatingRequiredDailyCalories(AnthropometricInfoModel infoModel, int currentCalories, DietGoalsEnum goal, out bool itIsMinAllowedValue);
        IDictionary<NutrientsEnum, int> GetNutrientsRates(int calories, int height, GenderEnum gender, DietGoalsEnum goal);
        bool CreateDietPlan(DietPreferencesModel model);
        UserAnthropometricAndDietModel GetAnthropometricAndDietModel(string userId);

        //Python using
        IEnumerable<string> GetAllProducts(string pythonFile);
        ProductNutrientsModel GetProductInfoByName(string pythonFile, string productName);

    }
}
