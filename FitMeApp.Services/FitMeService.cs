using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;


namespace FitMeApp.Services
{
    public class FitMeService: IFitMeService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;
        private readonly ILogger _logger;
        public FitMeService(IRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger("FitMeServiceLogger");
            _mapper = new EntityModelMapper(loggerFactory);
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
                    gymsModels.Add(_mapper.MappGymEntityBaseToModelBase(gym));
                }
                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

        }

        public GymModel GetGymModel(int id)
        {
            var gymEntityBase = _repository.GetGymWithTrainersAndTrainings(id);           
            GymModel gym = _mapper.MappGymEntityBaseToModel(gymEntityBase);
            return gym;
        }


        public IEnumerable<GymModel> GetGymsByTrainings(List<int> trainingsId)
        {
            var gymsByTrainings = _repository.GetGymsByTrainings(trainingsId);
            List<GymModel> gyms = new List<GymModel>();
            foreach (var gym in gymsByTrainings)
            {
                gyms.Add(_mapper.MappGymEntityBaseToModelBase(gym));
            }
            return gyms;
        }

        public IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId)
        {
            var workHoursEntityBase = _repository.GetWorkHoursByGym(gymId);
            List<GymWorkHoursModel> workHoursModels = new List<GymWorkHoursModel>();
            foreach (var item in workHoursEntityBase)
            {
                workHoursModels.Add(_mapper.MappGymWorkHoursEntityBaseToModel(item));
            }
            return workHoursModels;
        }


        //Training

        public ICollection<TrainingModel> GetAllTrainingModels()
        {
            try
            {
                var trainings = _repository.GetAllTrainings();
                var trainingModels = new List<TrainingModel>();

                foreach (var groupClass in trainings)
                {                    
                    trainingModels.Add(_mapper.MappTrainingEntityBaseToModelBase(groupClass));
                }
                return trainingModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public TrainingModel GetTrainingModel(int trainingId)
        {
            var trainingEntity = _repository.GetTraining(trainingId);
            TrainingModel trainingModel = _mapper.MappTrainingEntityBaseToModelBase(trainingEntity);
            return trainingModel;
        }



      


        //Subscriptions

        public IEnumerable<SubscriptionModel> GetSubscriptionsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetSubscriptionsByGymByFilter(gymId, periods, groupTraining, dietMonitoring);
                foreach (var subscription in subscriptionsEntity)
                {
                    subscriptionsModels.Add(_mapper.MappSubscriptionPriceEntityBaseToModel(subscription));
                }
                return subscriptionsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

        }


        public IEnumerable<SubscriptionModel> GetAllSubscriptionsByGym(int gymId)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetAllSubscriptionsByGym(gymId);
                foreach (var subscription in subscriptionsEntity)
                {
                    subscriptionsModels.Add(_mapper.MappSubscriptionPriceEntityBaseToModel(subscription));
                }
                return subscriptionsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
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
            SubscriptionModel subscriptionModel = _mapper.MappSubscriptionPriceEntityBaseToModel(subscriptionPriceEntityBase);
            return subscriptionModel;
        }


        public bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate)
        {
            bool result = _repository.AddUserSubscription(userId, gymId, subscriptionId, startDate);
            return result;
        }

        //Trainers

        public List<TrainerModel> GetAllTrainerModels()
        {
            var trainersEntity = _repository.GetAllTrainersWithGymAndTrainings();
            List<TrainerModel> trainers = new List<TrainerModel>();
            foreach (var trainerEntity in trainersEntity)
            {
                trainers.Add(_mapper.MappTrainerWithGymAndTrainingsBaseToModel(trainerEntity));
            }
            return trainers;
        }

        public TrainerModel GetTrainerWithGymAndTrainings(string trainerId)
        {
            var trainerWithGymAndTrainings = _repository.GetTrainerWithGymAndTrainings(trainerId);
            TrainerModel trainer = _mapper.MappTrainerWithGymAndTrainingsBaseToModel(trainerWithGymAndTrainings);
            return trainer;
        }


        public IEnumerable<TrainerWorkHoursModel> GetWorkHoursByTrainer(string trainerId)
        {
            var workHoursEntityBase = _repository.GetWorkHoursByTrainer(trainerId);
            List<TrainerWorkHoursModel> workHoursModels = new List<TrainerWorkHoursModel>();
            foreach (var item in workHoursEntityBase)
            {
                workHoursModels.Add(_mapper.MappTrainerWorkHoursWithDaysBaseToModel(item));
            }
            return workHoursModels;
        }


        public bool UpdateTrainerWithGymAndTrainings(TrainerModel newTrainerInfo)
        {
            var trainerBase = _mapper.MappTrainerModelToBase(newTrainerInfo);
            bool result = _repository.UpdateTrainerWithGymAndTrainings(trainerBase);
            return result;
        }

        //Events
        public int GetActualEventsCountByTrainer(string trainerId)
        {
            int actualEventsCount = _repository.GetActualEventsCountByTrainer(trainerId);
            return actualEventsCount;
        }

    }
}
