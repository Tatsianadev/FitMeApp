using System.Collections.Generic;
using FitMeApp.Common;


namespace FitMeApp.Services.Contracts.Models
{
    public class TrainingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<TrainerModel> Trainers { get; set; }
        //public IEnumerable<GymModel> Gyms { get; set; }

    }
}
