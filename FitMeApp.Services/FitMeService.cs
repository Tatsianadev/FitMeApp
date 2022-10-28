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

        public IEnumerable<GymModel> GetAllGymModels()
        {
            try
            {
                var gymEntityBases = _repository.GetAllGyms();
                var gymsModels = new List<GymModel>();

                foreach (var gym in gymEntityBases)
                {
                    var trainers = _repository.GetTrainersOfGym(gym.Id);
                    var groupClasses = _repository.GetGroupClassesOfGym(gym.Id);
                    gymsModels.Add(_mapper.ConvertToGymModel(gym, trainers, groupClasses));
                }

                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            
        }

        public IEnumerable<TrainerModel> GetAllTrainerModels()
        {
            try
            {
                var trainers = _repository.GetAllTrainers();
                var trainerModels = new List<TrainerModel>();
                foreach (var trainer in trainers)
                {
                    var gyms = _repository.GetGymsOfTrainer(trainer.Id);
                    var groupClasses = _repository.GetGroupClassesOfTrainer(trainer.Id);
                    trainerModels.Add(_mapper.ConvertToTrainerModel(trainer, gyms, groupClasses));
                }

                return trainerModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }            
        }


        public ICollection<GroupClassModel> GetAllGroupClassModels()
        {
            try
            {
                var groupClasses = _repository.GetAllGroupClasses();
                var groupClassModels = new List<GroupClassModel>();

                foreach (var groupClass in groupClasses)
                {
                    var trainers = _repository.GetTrainersOfGroupClass(groupClass.Id);
                    var gyms = _repository.GetGymsOfGroupClass(groupClass.Id);
                    groupClassModels.Add(_mapper.GetGroupClassModel(groupClass, trainers, gyms));
                }

                return groupClassModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            
            
        }
    }
}
