using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Services
{
    public class SubscriptionService: ISubscriptionService
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;
        private readonly EntityModelMapper _mapper;

        public SubscriptionService(IRepository repository, ILogger<SubscriptionService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = new EntityModelMapper();
        }

        //Subscriptions

        public IEnumerable<SubscriptionModel> GetSubscriptionsForVisitorsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetSubscriptionsForVisitorsByGymByFilter(gymId, periods, groupTraining, dietMonitoring);
                foreach (var subscription in subscriptionsEntity)
                {
                    subscriptionsModels.Add(_mapper.MapSubscriptionPriceEntityBaseToModel(subscription));
                }
                return subscriptionsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public IEnumerable<SubscriptionModel> GetAllSubscriptionsForVisitorsByGym(int gymId)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetAllSubscriptionsForVisitorsByGym(gymId);
                foreach (var subscription in subscriptionsEntity)
                {
                    subscriptionsModels.Add(_mapper.MapSubscriptionPriceEntityBaseToModel(subscription));
                }
                return subscriptionsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public IEnumerable<SubscriptionModel> GetAllSubscriptionsForTrainersByGym(int gymId)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetAllSubscriptionsForTrainersByGym(gymId);
                foreach (var subscription in subscriptionsEntity)
                {
                    subscriptionsModels.Add(_mapper.MapSubscriptionPriceEntityBaseToModel(subscription));
                }
                return subscriptionsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<int> GetAllSubscriptionPeriods()
        {
            List<int> allSubscriptionPeriods = _repository.GetAllSubscriptionPeriods().ToList();
            return allSubscriptionPeriods;
        }


        public SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId)
        {
            var subscriptionPriceEntityBase = _repository.GetSubscriptionWithPriceByGym(subscriptionId, gymId);
            SubscriptionModel subscriptionModel = _mapper.MapSubscriptionPriceEntityBaseToModel(subscriptionPriceEntityBase);
            return subscriptionModel;
        }


        //UserSubscriptions
        public int AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate)
        {
            int userSubscriptionId = _repository.AddUserSubscription(userId, gymId, subscriptionId, startDate);
            return userSubscriptionId;
        }


        public IEnumerable<UserSubscriptionModel> GetUserSubscriptions(string userId)
        {
            var userSubscriptionsBase = _repository.GetUserSubscriptionsFullInfo(userId);
            List<UserSubscriptionModel> userSubscriptionsModel = new List<UserSubscriptionModel>();
            foreach (var baseItem in userSubscriptionsBase)
            {
                userSubscriptionsModel.Add(_mapper.MapUserSubscriptionFullInfoBaseToModel(baseItem));
            }
            return userSubscriptionsModel;
        }


        public IEnumerable<UserSubscriptionModel> GetSubscriptionsByFilterByUser(string userId,
            List<SubscriptionValidStatusEnum> validStatuses, List<int> gymIds)
        {
            List<UserSubscriptionModel> subscriptions = new List<UserSubscriptionModel>();
            List<UserSubscriptionFullInfoBase> subscriptionsBase = new List<UserSubscriptionFullInfoBase>();

            foreach (var status in validStatuses)
            {
                if (status == SubscriptionValidStatusEnum.validNow)
                {
                    subscriptionsBase.AddRange(_repository.GetValidSubscriptionsByUserForGyms(userId, gymIds));
                }

                if (status == SubscriptionValidStatusEnum.expired)
                {
                    subscriptionsBase.AddRange(_repository.GetExpiredSubscriptionsByUserForGyms(userId, gymIds));
                }

                if (status == SubscriptionValidStatusEnum.validInTheFuture)
                {
                    subscriptionsBase.AddRange(_repository.GetValidInTheFutureSubscriptionsByUserForGyms(userId, gymIds));
                }
            }

            if (subscriptionsBase.Count == 0)
            {
                return subscriptions;
            }

            subscriptionsBase.Distinct();

            foreach (var subscriptionBase in subscriptionsBase)
            {
                subscriptions.Add(_mapper.MapUserSubscriptionFullInfoBaseToModel(subscriptionBase));
            }

            return subscriptions;
        }



        public void DeleteUserSubscription(int userSubscriptionId)
        {
            _repository.DeleteUserSubscription(userSubscriptionId);
        }


    }
}
