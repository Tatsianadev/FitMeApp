using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels.Chart
{
    [DataContract]
    public class TimeVisitorsViewModel
    {
        [DataMember(Name = "label")]
        public int Hour { get; set; }

        [DataMember(Name = "y")]
        public int NumberOfVisitors { get; set; }
    }
}
