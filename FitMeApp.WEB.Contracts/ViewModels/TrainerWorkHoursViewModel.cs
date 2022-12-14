using FitMeApp.WEB.Contracts.ViewModels.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class TrainerWorkHoursViewModel
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        //public int StartTime { get; set; }
        public string StartTime { get; set; }
        //public int EndTime { get; set; }        
        public string EndTime { get; set; }
        public int GymWorkHoursId { get; set; }
        public DayOfWeek DayName { get; set; }
    }
}
