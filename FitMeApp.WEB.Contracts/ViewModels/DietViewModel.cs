using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FitMeApp.Common;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class DietViewModel
    {
        public int Id { get; set; }
        public int CurrentCalorieIntake { get; set; }
        public DietGoalsEnum Goal { get; set; }
        public int RequiredCalorieIntake { get; set; }
        public bool ItIsMinAllowedCaloriesValue { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }
        public DateTime Date { get; set; }
    }
}
