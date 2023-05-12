using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;

namespace FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase
{
     public class GroupClassScheduleRecordFullInfo: GroupTrainingScheduleEntityBase
    {
        public string TrainerId { get; set; }
        public int GroupClassId { get; set; }
    }
}
