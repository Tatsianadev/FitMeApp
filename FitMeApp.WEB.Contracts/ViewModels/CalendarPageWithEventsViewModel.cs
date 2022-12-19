using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class CalendarPageWithEventsViewModel
    {       
        public DateTime Date { get; set; } //отображает страницу в календаре (Calendar partial view) и конкретный указатель даты в Events partial view 
        public bool DayOnCalendarSelected { get; set; } //флаг для отображения/скртытия Events partial view 
        public string MonthName { get; set; }       
        public IDictionary<DateTime, int> DatesEventsCount { get; set; } //нужна именно коллекция, поскольку на ОДНОЙ странице календаря должны отображаться события для всех дней в месяце 
        public ICollection<EventViewModel> Events { get; set; } //поле имеет значение только после выбора конкретной даты 
    }
}
