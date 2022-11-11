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

        //public IEnumerable<GymModel> GetAllGymModels()
        //{
        //    try
        //    {
        //        var gymEntityBases = _repository.GetAllGyms();
        //        var gymsModels = new List<GymModel>();

        //        foreach (var gym in gymEntityBases)
        //        {
        //            var gymWithTrainersAndTrainings = _repository.GetGymWithTrainersAndTrainings(gym.Id);
        //            gymsModels.Add(_mapper.MappGymEntityBaseToModel(gymWithTrainersAndTrainings));
        //        }

        //        return gymsModels;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw ex;
        //    }
            
        //}

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

        public IEnumerable<TrainerModel> GetAllTrainerModels()
        {
            try
            {
                var trainers = _repository.GetAllTrainers();
                var trainerModels = new List<TrainerModel>();
                foreach (var trainer in trainers)
                {
                    var trainerWithGymAndGroups = _repository.GetTrainerWithGymAndTrainings(trainer.Id);
                    trainerModels.Add(_mapper.MappTrainerEntityBaseToModel(trainerWithGymAndGroups));
                }

                return trainerModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }            
        }


        //public ICollection<TrainingModel> GetAllTrainingModels()
        //{
        //    try
        //    {
        //        var groupClasses = _repository.GetAllTrainings();
        //        var groupClassModels = new List<TrainingModel>();

        //        foreach (var groupClass in groupClasses)
        //        {
        //            var groupClassWithGymsAndTrainers = _repository.GetTrainingWithTrainerAndGym(groupClass.Id);
        //            groupClassModels.Add(_mapper.MappGroupClassEntityBaseToModel(groupClassWithGymsAndTrainers));
        //        }

        //        return groupClassModels;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        throw;
        //    }
            
            
        //}

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

        public IEnumerable<GymModel> GetGymsByTrainings(List<int> trainingsId)
        {
            var gymsByTrainings = _repository.GetGymsByTrainings(trainingsId);          
            List<GymModel> gyms = new List<GymModel>();
            foreach ( var gym in gymsByTrainings)
            {                
                gyms.Add(_mapper.MappGymEntityBaseToModelBase(gym));
            }
            return gyms;
        }


        public IEnumerable<SubscriptionModel> GetSubscriptionsByGym(int gymId)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetSubscriptionsByGym(gymId);
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

    }
}
