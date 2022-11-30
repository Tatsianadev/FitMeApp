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
        IEnumerable<EventModel> GetEventsByTrainerAndDate(string trainerId, DateTime dateTime);
        IDictionary<string, int> GetEventsCountForEachDateByUser(string userId);
        IDictionary<string, int> GetEventsCountForEachDateByTrainer(string trainerId);
        bool ChangeEventStatus(int eventId);
    }
}
