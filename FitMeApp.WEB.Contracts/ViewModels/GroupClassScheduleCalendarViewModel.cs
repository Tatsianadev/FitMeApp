using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class GroupClassScheduleCalendarViewModel
    {
        public int GroupClassId { get; set; }
        public string GroupClassName { get; set; }
        public string TrainerId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public IEnumerable<GroupClassScheduleViewModel> DailyInfo { get; set; }
    }
}
