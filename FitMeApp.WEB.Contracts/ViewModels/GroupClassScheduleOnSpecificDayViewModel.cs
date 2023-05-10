﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class GroupClassScheduleOnSpecificDayViewModel
    {
        public DayOfWeek DayOfWeekName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ParticipantsLimit { get; set; }
        public int ActualParticipantsCount { get; set; }
    }
}
