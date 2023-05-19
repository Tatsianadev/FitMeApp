using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class GroupClassScheduleRecordViewModel
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int GroupClassId { get; set; }
        public string GroupClassName { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ParticipantsLimit { get; set; }
        public int ActualParticipantsCount { get; set; }
    }
}
