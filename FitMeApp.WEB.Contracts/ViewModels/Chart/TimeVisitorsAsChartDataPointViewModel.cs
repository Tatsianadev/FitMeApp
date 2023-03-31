using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels.Chart
{
    [DataContract]
    public class TimeVisitorsAsChartDataPointViewModel
    {
        [DataMember(Name = "label")]
        public string Hour { get; set; }

        [DataMember(Name = "y")]
        public int NumberOfVisitors { get; set; }

    }
}
