using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FitMeApp.WEB.Contracts.ViewModels;

namespace FitMeApp.WEB.Contracts.Attributes
{
    public class StartDateEarlierThanEndAttribute: ValidationAttribute
    {
        public StartDateEarlierThanEndAttribute()
        {
            ErrorMessage = "The date of the start contract cannot be later or equal to the expiration date.";
        }

        public override bool IsValid(object? value)
        {
            if (value is TrainerWorkLicenseViewModel model)
            {
                if (model.StartDate < model.EndDate)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
