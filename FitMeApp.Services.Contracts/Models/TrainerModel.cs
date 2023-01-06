using FitMeApp.Common;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Models
{
    public class TrainerModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public string Specialization { get; set; }
        public TrainerApproveStatusEnum Status { get; set; }
        public int GymId { get; set; }
        public GymModel Gym { get; set; }       
        public ICollection<TrainingModel> Trainings { get; set; }
    }
}
