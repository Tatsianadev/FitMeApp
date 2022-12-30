using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class TrainerWithGymAndTrainingsBase
    {
        [Key]
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public string Specialization { get; set; }        
        public GymEntityBase Gym { get; set; }      
        public IEnumerable<TrainingEntityBase> Trainings { get; set; }
        public TrainerConfirmStatusEnum Status { get; set; }
    }
}
