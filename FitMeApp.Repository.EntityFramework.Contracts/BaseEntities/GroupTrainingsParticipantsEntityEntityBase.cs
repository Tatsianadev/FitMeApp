﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class GroupTrainingsParticipantsEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int WeeklyGroupTrainingsScheduleId { get; set; }
        public string UserId { get; set; }
    }
}
