using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GroupTrainingsParticipants")]
    public class GroupTrainingsParticipantEntity: GroupClassParticipantEntityBase
    {
        public virtual GroupTrainingScheduleEntity GroupTrainingsSchedule { get; set; }
        public virtual User Users { get; set; }
    }
}
