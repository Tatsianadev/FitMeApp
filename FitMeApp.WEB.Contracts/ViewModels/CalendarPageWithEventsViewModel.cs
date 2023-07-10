using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class CalendarPageWithEventsViewModel
    {       
        public DateTime Date { get; set; } //The property displays the page in the calendar (Calendar partial view) and the specific date pointer in the Events partial view
        public bool DayOnCalendarSelected { get; set; } //flag to show/hide Events partial view 
        public bool SelectedDayIsWorkOff { get; set; } //flag to redirect to WorkOff partial view       
        public string MonthName { get; set; }       
        public IDictionary<DateTime, int> DatesEventsCount { get; set; } //collection because on th ONE page of the calendar should be shown events for all days in the month 
        public ICollection<EventViewModel> Events { get; set; } //the field is only meaningful after a specific date has been selected
    }
}
