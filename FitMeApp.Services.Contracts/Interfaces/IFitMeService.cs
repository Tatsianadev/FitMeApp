using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFitMeService
    {
        //Gyms
        IEnumerable<GymModel> GetAllGymModels();
        IEnumerable<GymModel> GetAllGymsWithGalleryModels();
        GymModel GetGymModel(int id);
        IEnumerable<GymModel> GetGymsByTrainings(List<int> groupClassesId);
        IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId);
        int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek);
        int GetGymIdByTrainer(string trainerId);    

        //Trainings
        ICollection<TrainingModel> GetAllTrainingModels();
        TrainingModel GetTrainingModel(int trainingId);

        //Subscriptions
        IEnumerable<SubscriptionModel> GetSubscriptionsForVisitorsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsForVisitorsByGym(int gymId);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsForTrainersByGym(int gymId);
        List<int> GetAllSubscriptionPeriods();
        SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId);

        //UserSubscriptions
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        //int GetActualSubscriptionsCountByTrainer(string trainerId);
        IEnumerable<UserSubscriptionModel> GetUserSubscriptions(string userId);
        IEnumerable<UserSubscriptionModel> GetSubscriptionsByFilterByUser(string userId, List<SubscriptionValidStatusEnum> validStatuses, List<int> gymIds);

        //Trainers
       
        

        //Events
        int GetActualEventsCountByTrainer(string trainerId);


        //TrainingTrainer ???
        void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId);
        bool AddTrainingTrainerConnection(string trainerId, int trainingId);


    }
}
