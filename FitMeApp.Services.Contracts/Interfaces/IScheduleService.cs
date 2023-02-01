using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IScheduleService
    {
        IEnumerable<PersonalTrainingEventModel> GetAllEvents();
        IEnumerable<PersonalTrainingEventModel> GetEventsByUser(string userId);
        IEnumerable<PersonalTrainingEventModel> GetEventsByUserAndDate(string userId, DateTime dateTime);
        IEnumerable<PersonalTrainingEventModel> GetEventsByTrainerAndDate(string trainerId, DateTime dateTime);
        IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId);
        IDictionary<DateTime, int> GetEventsCountForEachDateByTrainer(string trainerId);
        bool ChangeEventStatus(int eventId);
    }
}
