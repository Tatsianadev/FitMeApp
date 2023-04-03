using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models.Chart
{
    public class VisitingChartModel
    {
        public int GymId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public List<VisitorsPerHourModel> TimeVisitorsLine { get; set; }
    }
}
