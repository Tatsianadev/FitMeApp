using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Trainings")]
    public class TrainingEntity : TrainingEntityBase
    {
        public TrainingEntity()
        {
            this.Gyms = new HashSet<GymEntity>();
            this.Trainers = new HashSet<TrainerEntity>();


        }

        public ICollection<GymEntity> Gyms { get; set; }
        public ICollection<TrainerEntity> Trainers { get; set; }

    }
}
