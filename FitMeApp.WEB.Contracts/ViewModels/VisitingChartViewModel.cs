using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.WEB.Contracts.ViewModels.Chart;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class VisitingChartViewModel
    {
        public int GymId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public List<TimeVisitorsViewModel> TimeVisitorsLine { get; set; }
    }
}
