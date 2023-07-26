using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Models.JsonModels;

namespace FitMeApp.Services.Contracts.Models
{
    public class HomeTrainingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AverageCalConsumption { get; set; }
        public int AgeUpTo { get; set; }
        public string Gender { get; set; }
        public int Duration { get; set; }
        public int BMIfrom { get; set; }
        public int BMIupTo { get; set; }
        public bool EquipmentIsNeeded { get; set; }
        public string ShortDescription { get; set; }
    }
}
