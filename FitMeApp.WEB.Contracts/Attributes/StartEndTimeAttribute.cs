using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FitMeApp.WEB.Contracts.ViewModels;

namespace FitMeApp.WEB.Contracts.Attributes
{
    public class StartEarlierThanEndTimeAttribute : ValidationAttribute
    {
        public StartEarlierThanEndTimeAttribute()
        {
            ErrorMessage = "Start time of work can't be less than End time of work";
        }
        public override bool IsValid(object? value)
        {
            if (value is TrainerWorkHoursViewModel model)
            {
                int startTimeInt = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(model.StartTime);
                int endTimeInt = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(model.EndTime);

                return startTimeInt <= endTimeInt;
            }

            return false;
        }
    }
}
