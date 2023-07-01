using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class DietPdfReportModel
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string Gender { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Age { get; set; }
        public int PhysicalActivity { get; set; }
        public DateTime AnthropometricInfoDate { get; set; }
        public int CurrentCalorieIntake { get; set; }
        public int DietGoalId { get; set; }
        public int RequiredCalorieIntake { get; set; }
        public int Proteins { get; set; }
        public int Fats { get; set; }
        public int Carbohydrates { get; set; }
        public int Budget { get; set; }
        public DateTime DietPlanCreatedDate { get; set; }
        
    }
}
