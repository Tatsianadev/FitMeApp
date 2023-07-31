using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class HomeTrainingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AverageCalConsumption { get; set; }
        public int MaxAge { get; set; }
        public string Gender { get; set; }
        public int Duration { get; set; }
        public int MaxBMI { get; set; }
        public bool EquipmentIsNeeded { get; set; }
        public string ShortDescription { get; set; }
        public string TrainingPlanFile { get; set; }
    }
}
