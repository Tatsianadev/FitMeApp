using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models.Chart
{
    public class VisitorsPerHourModel
    {
        public int Hour { get; set; }
        public int NumberOfVisitors { get; set; }
    }
}
