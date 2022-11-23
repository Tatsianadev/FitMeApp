using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IScheduleService
    {
        IEnumerable<EventModel> GetAllEvents();
        IEnumerable<EventModel> GetEventsByUser(string userId);
        IEnumerable<EventModel> GetEventsByUserAndDate(string userId, DateTime dateTime);
    }
}
