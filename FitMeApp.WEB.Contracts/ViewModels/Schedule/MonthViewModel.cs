using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels.Schedule
{
    public class MonthViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DaysNumber { get; set; }
        public string FirstDayName { get; set; }
        public string LastDayName { get; set; }
    }
}
