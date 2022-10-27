using FitMeApp.Contracts.Interfaces;
using FitMeApp.Services.Interfaces;
using FitMeApp.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services
{
    public class FitMeService: IFitMeService
    {
        private readonly IRepository _repository;
        private readonly Converter _converter;
        private readonly ILogger _logger;
        public FitMeService(IRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger("FitMeServiceLogger");
            _converter = new Converter(loggerFactory);
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
                    gymsModels.Add(_converter.ConvertToGymModel(gym, trainers, groupClasses));
                }

                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            
        }

        public IEnumerable<TrainerModel> GetAllTrainers()
        {
            try
            {
                var trainers = _repository.GetAllTrainers();
                var trainerModels = new List<TrainerModel>();
                foreach (var trainer in trainers)
                {
                    var gyms = _repository.GetGymsOfTrainer(trainer.Id);
                    var groupClasses = _repository.GetGroupClassesOfTrainer(trainer.Id);
                    trainerModels.Add(_converter.ConvertToTrainerModel(trainer, gyms, groupClasses));
                }

                return trainerModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
            
        }
    }
}
