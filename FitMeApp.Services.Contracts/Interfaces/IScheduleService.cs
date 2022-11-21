using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IScheduleService
    {
        string GetMonthName(int month);
        int GetMonthDaysNumber(int month);
        string GetFirstDayName(int month);
        string GetLastDayName(int month);
    }
}
