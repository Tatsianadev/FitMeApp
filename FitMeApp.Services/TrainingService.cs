using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;

namespace FitMeApp.Services
{
    public class TrainingService: ITrainingService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;

        public TrainingService(IRepository repository)
        {
            _repository = repository;
            _mapper = new EntityModelMapper();
        }



        public IEnumerable<int> GetAvailableToApplyTrainingTimingByTrainer(string trainerId, DateTime date)
        {
            List<int> availableTimeInMinutes = _repository.GetAvailableToApplyTrainingTimingByTrainer(trainerId, date).ToList();
            return availableTimeInMinutes;
        }
    }
}
