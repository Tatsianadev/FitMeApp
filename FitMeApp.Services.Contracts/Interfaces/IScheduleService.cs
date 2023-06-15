using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IScheduleService
    {
        IEnumerable<EventModel> GetAllEvents();
        EventModel GetEvent(int eventId);
        int AddEvent(EventModel newEvent);
        void DeleteEvent(int eventId);
        IEnumerable<EventModel> GetEventsByUser(string userId);
        IEnumerable<EventModel> GetEventsByUserAndDate(string userId, DateTime dateTime);
        IEnumerable<EventModel> GetPersonalTrainingsByTrainerAndDate(string trainerId, DateTime dateTime);
        IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId);
        IDictionary<DateTime, int> GetEventsCountForEachDateByTrainer(string trainerId);
        void ConfirmEvent(int eventId);
        int GetActualEventsCountByTrainer(string trainerId);
        int GetOpenedEventsCountByTrainer(string trainerId);
        int GetEventsCount(string trainerId, List<DateTime> dates, int startTime, int endTime);
        bool CheckIfNoEventsAtSelectedTime(string userId, int selectedStartTime, int selectedEndTime, DateTime date);
        bool CheckIfNoGroupClassesAtSelectedTime(string trainerId, int selectedStartTime, int selectedEndTime, DateTime date);

    }
}
