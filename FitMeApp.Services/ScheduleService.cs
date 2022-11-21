using FitMeApp.Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FitMeApp.Services
{
    public class ScheduleService: IScheduleService
    {
        public string GetMonthName(int month)
        {  
            string monthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month);
            return monthName;
        }

        public int GetMonthDaysNumber(int month)
        {
            int daysNumber = DateTime.DaysInMonth(2022, month);
            return daysNumber;
        }
    }
}
