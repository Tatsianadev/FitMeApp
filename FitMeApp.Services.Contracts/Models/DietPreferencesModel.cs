using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class DietPreferencesModel
    {
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public NutrientsModel AllNutrients { get; set; }
        public NutrientsModel LovedNutrients { get; set; }
        public NutrientsModel UnlovedNutrients { get; set; }
        public List<string> AllergicTo { get; set; }
        public int Budget { get; set; }

    }
}
