using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class NutrientsModel
    {
        public List<string> Proteins { get; set; }
        public List<string> Fats { get; set; }
        public List<string> Carbohydrates { get; set; }
    }
}
