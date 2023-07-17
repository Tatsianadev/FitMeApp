using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models.Chart
{
    public class ProductNutrientsModel
    {
        public string Name { get; set; }
        public int Calorie { get; set; }
        public decimal Protein { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
    }
}
