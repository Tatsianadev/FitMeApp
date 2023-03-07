using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Trainers")]
    public class TrainerEntity: TrainerEntityBase
    {
        public TrainerEntity()
        {            
            Trainings = new HashSet<TrainingEntity>();
        }

        
        public GymEntity Gym { get; set; }
        public TrainerWorkLicenseEntity WorkLicense { get; set; }
        public ICollection<TrainingEntity> Trainings { get; set; }
    }
}
