using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ITrainingService
    {
        IEnumerable<int> GetAvailableTimeForTraining (string trainerId, DateTime date);
        bool CheckIfUserHasAvailableSubscription(string userId, DateTime trainingDate);
        bool AddEvent(EventModel newEvent);

    }
}
