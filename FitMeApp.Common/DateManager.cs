using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FitMeApp.Common
{
    public static class DateManager
    {
        public static IEnumerable<DateTime> GetDatesInSpanByDayOfWeek(DateTime dateStart, int daysSpan, string dayOfWeekName)
        {
            int year = dateStart.Year;
            int month = dateStart.Month;
            int dayNumber = dateStart.Day;

            List<DateTime> dates = Enumerable.Range(1, daysSpan)
                .Where(d => new DateTime(year, month, dayNumber).AddDays(d - 1).ToString("dddd").Equals(dayOfWeekName))
                .Select(d => new DateTime(year, month, dayNumber).AddDays(d - 1)).ToList();

            return dates;
        }
    }
}
