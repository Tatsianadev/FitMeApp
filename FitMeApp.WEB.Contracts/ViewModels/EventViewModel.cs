﻿using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class EventViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public string TrainerId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }

        public int GymId { get; set; }
        public string GymName { get; set; }

        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }

        public int TrainingId { get; set; }
        public string TrainingName { get; set; }
        public int GroupClassScheduleRecordId { get; set; }
        public int ParticipantsLimit { get; set; }
        public int ActualParticipantsCount { get; set; }
        public EventStatusEnum Status { get; set; }
    }
}
