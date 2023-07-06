using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class UserAnthropometricAndDietViewModel
    {
        public List<AnthropometricInfoViewModel> AnthropometricInfo { get; set; }
        public DietViewModel DietParameters { get; set; }
        public string DietReportRelativePath { get; set; }
      
    }
}
