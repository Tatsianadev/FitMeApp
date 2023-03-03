﻿using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;

namespace FitMeApp.Services
{
    public class FitMeService: IFitMeService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;
        private readonly ILogger _logger;
        public FitMeService(IRepository repository, ILogger<FitMeService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = new EntityModelMapper();
        }

        //Gym
        
        public IEnumerable<GymModel> GetAllGymModels()
        {
            try
            {
                var gymEntityBases = _repository.GetAllGyms();
                var gymsModels = new List<GymModel>();

                foreach (var gym in gymEntityBases)
                {                    
                    gymsModels.Add(_mapper.MapGymEntityBaseToModelBase(gym));
                }
                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }


        public IEnumerable<GymModel> GetAllGymsWithGalleryModels()
        {
            try
            {
                var gymEntityBases = _repository.GetAllGymsWithGallery();
                var gymsModels = new List<GymModel>();

                foreach (var gym in gymEntityBases)
                {
                    gymsModels.Add(_mapper.MapGymWithGalleryBaseToModelBase(gym));
                }
                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        public GymModel GetGymModel(int id)
        {
            var gymEntityBase = _repository.GetGymWithTrainersAndTrainings(id);           
            GymModel gym = _mapper.MapGymEntityBaseToModel(gymEntityBase);
            return gym;
        }


        public IEnumerable<GymModel> GetGymsByTrainings(List<int> trainingsId)
        {
            var gymsByTrainings = _repository.GetGymsByTrainings(trainingsId);
            List<GymModel> gyms = new List<GymModel>();
            foreach (var gym in gymsByTrainings)
            {
                gyms.Add(_mapper.MapGymEntityBaseToModelBase(gym));
            }
            return gyms;
        }

        public IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId)
        {
            var workHoursEntityBase = _repository.GetWorkHoursByGym(gymId);
            List<GymWorkHoursModel> workHoursModels = new List<GymWorkHoursModel>();
            foreach (var item in workHoursEntityBase)
            {
                workHoursModels.Add(_mapper.MapGymWorkHoursEntityBaseToModel(item));
            }
            return workHoursModels;
        }

        public int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek)
        {
            int gymWorkHoursId = _repository.GetGymWorkHoursId(gymId, dayOfWeek);
            return gymWorkHoursId;
        }

        public int GetGymIdByTrainer(string trainerId)
        {
            int gymId = _repository.GetTrainer(trainerId).GymId;
            return gymId;
        }



        //Training

        

        

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

        public List<int> GetAllSubscriptionPeriods()
        {
            List<int> allSubscriptionPeriods = _repository.GetAllSubscriptionPeriods();
            return allSubscriptionPeriods;
        }


        public SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId)
        {
            var subscriptionPriceEntityBase = _repository.GetSubscriptionWithPriceByGym(subscriptionId, gymId);
            SubscriptionModel subscriptionModel = _mapper.MapSubscriptionPriceEntityBaseToModel(subscriptionPriceEntityBase);
            return subscriptionModel;
        }

        //UserSubscriptions
        public bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate)
        {
            bool result = _repository.AddUserSubscription(userId, gymId, subscriptionId, startDate);
            return result;
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



        //Trainers
        


        //Events
        public int GetActualEventsCountByTrainer(string trainerId)
        {
            int actualEventsCount = _repository.GetActualEventsCountByTrainer(trainerId);
            return actualEventsCount;
        }

        //TrainingTrainer
        public void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId)
        {
            _repository.DeleteAllTrainingTrainerConnectionsByTrainer(trainerId);
        }

        public bool AddTrainingTrainerConnection(string trainerId, int trainingId)
        {
            bool result = _repository.AddTrainingTrainerConnection(trainerId, trainingId);
            return result;
        }

    }
}
