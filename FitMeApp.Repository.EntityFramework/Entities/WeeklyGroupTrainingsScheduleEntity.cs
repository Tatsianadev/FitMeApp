using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("WeeklyGroupTrainingsSchedule")]
    public class WeeklyGroupTrainingsScheduleEntity: WeeklyGroupTrainingsScheduleEntityBase
    {
        public virtual GymEntity Gyms { get; set; }
        public virtual TrainingTrainerEntity TrainingTrainer { get; set; }
    }
}
