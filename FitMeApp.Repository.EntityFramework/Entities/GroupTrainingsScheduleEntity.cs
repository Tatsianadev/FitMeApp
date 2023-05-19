using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GroupTrainingsSchedule")]
    public class GroupTrainingScheduleEntity: GroupClassScheduleRecordEntityBase
    {
        public virtual GymEntity Gym { get; set; }
        public virtual TrainingTrainerEntity TrainingTrainer { get; set; }
        public ICollection<GroupTrainingsParticipantEntity> GroupTrainingsParticipants { get; set; }

        public GroupTrainingScheduleEntity()
        {
            GroupTrainingsParticipants = new HashSet<GroupTrainingsParticipantEntity>();
        }
    }
}
