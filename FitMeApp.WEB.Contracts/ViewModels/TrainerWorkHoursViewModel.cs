using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.WEB.Contracts.Attributes;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    [StartEarlierThanEndTime]
    public class TrainerWorkHoursViewModel
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int GymWorkHoursId { get; set; }
        public DayOfWeek DayName { get; set; }

    }
}
