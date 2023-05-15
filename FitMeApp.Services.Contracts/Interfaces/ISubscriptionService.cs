using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
     public interface ISubscriptionService
    {
        //Subscriptions
        IEnumerable<SubscriptionModel> GetSubscriptionsForVisitorsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsForVisitorsByGym(int gymId);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsForTrainersByGym(int gymId);
        IEnumerable<int> GetAllSubscriptionPeriods();
        SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId);


        //UserSubscriptions
        int AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        IEnumerable<UserSubscriptionModel> GetUserSubscriptions(string userId);
        IEnumerable<UserSubscriptionModel> GetSubscriptionsByFilterByUser(string userId, List<SubscriptionValidStatusEnum> validStatuses, List<int> gymIds);
        void DeleteUserSubscription(int userSubscriptionId);
    }
}
