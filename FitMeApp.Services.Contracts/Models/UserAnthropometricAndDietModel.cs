using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class UserAnthropometricAndDietModel
    {
        public List<AnthropometricInfoModel> AnthropometricInfo { get; set; }
        public DietModel DietParameters { get; set; }
    }
}
