using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase
{
    public class TrainerWorkHoursWithDayBase
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int GymWorkHoursId { get; set; }
        public DayOfWeek DayName { get; set; }
    }
}
