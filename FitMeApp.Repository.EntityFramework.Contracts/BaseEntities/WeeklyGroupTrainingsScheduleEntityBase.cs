﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class WeeklyGroupTrainingsScheduleEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int TrainingTrainerId { get; set; }
        public int GymId { get; set; }
        public int DayOfWeekNumber { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int ParticipantsLimit { get; set; }
    }
}
