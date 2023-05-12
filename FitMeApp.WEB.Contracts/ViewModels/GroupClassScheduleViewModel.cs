using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class GroupClassScheduleViewModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int ParticipantsLimit { get; set; }
        public int ActualParticipantsCount { get; set; }
    }
}
