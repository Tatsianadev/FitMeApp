using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class GymWorkHoursModel
    {        
        public int Id { get; set; }
        public DayOfWeek DayName { get; set; }
        public int GymId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
