using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class TrainerWorkHoursModel
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int GymWorkHoursId { get; set; }
        public DayOfWeek DayName { get; set; }
    }
}
