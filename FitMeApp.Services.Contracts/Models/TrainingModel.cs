using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Models
{
    public class TrainingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TrainerModel> Trainers { get; set; }
        public ICollection<GymModel> Gyms { get; set; }

    }
}
