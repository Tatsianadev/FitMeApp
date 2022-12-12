using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.WEB.Contracts.ViewModels.Attributes
{
    //It was created to validate TrainerWorkHoursViewModel, but there is problem to pass StartTime property to Attribute Constructor 
    public class EndTimeAttribute : ValidationAttribute
    {
        string _startTime;        

        public EndTimeAttribute(string startTime)
        {
            _startTime = startTime;
            
        }

        public override bool IsValid(object? value)
        {
            int startTimeInt = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(_startTime);
            int endTimeInt = Common.WorkHoursTypesConverter.ConvertStringTimeToInt((string)value);
            return startTimeInt >= endTimeInt && endTimeInt >= 0;
        }


    }


}
