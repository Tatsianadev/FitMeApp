using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class GymWorkHoursViewModel
    {
        public int Id { get; set; }
        public DayOfWeek DayName { get; set; }
        public int GymId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
