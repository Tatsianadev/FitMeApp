using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFitMeService
    {
        IEnumerable<GymModel> GetAllGymModels();
        GymModel GetGymModel(int id);
        IEnumerable<GymModel> GetGymsByTrainings(List<int> groupClassesId);
        IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId);

        //Trainings
        ICollection<TrainingModel> GetAllTrainingModels();
        IEnumerable<TrainerWorkHoursModel> GetWorkHoursByTrainer(string trainerId);

        //Subscriptions
        IEnumerable<SubscriptionModel> GetSubscriptionsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsByGym(int gymId);
        List<int> GetAllSubscriptionPeriods();

        SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId);
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);

        //Trainers
        List<TrainerModel> GetAllTrainerModels();
        TrainerModel GetTrainerWithGymAndTrainings(string trainerId);

    }
}
