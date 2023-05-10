using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GroupTrainingsParticipants")]
    public class GroupTrainingsParticipantsEntity: GroupTrainingsParticipantsEntityBase
    {
        public virtual GroupTrainingsScheduleEntity WeeklyGroupTrainingsSchedule { get; set; }
        public virtual User Users { get; set; }
    }
}
