using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class DietPreferencesViewModel
    {
        public NutrientsModel AllNutrients { get; set; }
        public NutrientsModel LovedNutrients { get; set; }
        public NutrientsModel UnlovedNutrients { get; set; }
        public NutrientsModel AllergicTo { get; set; }
        public int Budget { get; set; }
    }
}
