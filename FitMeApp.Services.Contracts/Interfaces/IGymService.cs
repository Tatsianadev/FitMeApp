using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IGymService
    {
        //Gyms
        IEnumerable<GymModel> GetAllGymModels();
        IEnumerable<GymModel> GetAllGymsWithGalleryModels();
        GymModel GetGymModel(int id);
        IEnumerable<GymModel> GetGymsByTrainings(List<int> groupClassesId);
        IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId);
        int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek);
        int GetGymIdByTrainer(string trainerId);    

        
        //Subscriptions
        IEnumerable<SubscriptionModel> GetSubscriptionsForVisitorsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsForVisitorsByGym(int gymId);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsForTrainersByGym(int gymId);
        List<int> GetAllSubscriptionPeriods();
        SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId);


        //UserSubscriptions
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        IEnumerable<UserSubscriptionModel> GetUserSubscriptions(string userId);
        IEnumerable<UserSubscriptionModel> GetSubscriptionsByFilterByUser(string userId, List<SubscriptionValidStatusEnum> validStatuses, List<int> gymIds);

        
    }
}
