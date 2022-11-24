using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class CurrentDayEventsViewModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int  Day { get; set; }
        public string DayName { get; set; }
        public ICollection<EventViewModel> Events { get; set; }
    }
}
