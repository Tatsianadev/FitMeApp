using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models.Chart
{
    public class AttendanceChartModel
    {
        public int GymId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public List<VisitorsPerHourModel> NumberOfVisitorsPerHour { get; set; }
    }
}
