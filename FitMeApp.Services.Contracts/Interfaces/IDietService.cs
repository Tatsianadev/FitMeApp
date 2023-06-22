using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IDietService
    {
        int AddAnthropometricInfo(AnthropometricInfoModel infoModel);
        int CalculatingCurrentDailyCalories(AnthropometricInfoModel infoModel);
        int CalculatingNeededDailyCalories(AnthropometricInfoModel infoModel, int currentCalories, DietGoalsEnum goal, out bool itIsMinAllowedValue);

    }
}
