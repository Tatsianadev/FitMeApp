using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("TrainingTrainer")]
    public class TrainingTrainerEntity
    {
        [Key]
        public int Id { get; set; }
        public int TrainingId { get; set; }        
        public string TrainerId { get; set; }
        public virtual TrainingEntity Training { get; set; }       
        public virtual TrainerEntity Trainer { get; set; }

        public ICollection<WeeklyGroupTrainingsScheduleEntity> WeeklyGroupTrainingsSchedule { get; set; }

        public TrainingTrainerEntity()
        {
            WeeklyGroupTrainingsSchedule = new HashSet<WeeklyGroupTrainingsScheduleEntity>();
        }
    }
}
