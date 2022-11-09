using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Trainings")]
    public class TrainingEntity: TrainingEntityBase
    {
        public TrainingEntity()
        {
                  
        }
        
        public virtual GymEntity Gym { get; set; }
        public virtual TrainerEntity Trainer { get; set; }

    }
}
