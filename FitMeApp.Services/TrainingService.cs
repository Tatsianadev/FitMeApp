using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;

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



        public IEnumerable<int> GetAvailableTimeForTraining(string trainerId, DateTime date)
        {
            List<int> availableTimeInMinutes = _repository.GetAvailableToApplyTrainingTimeByTrainer(trainerId, date).ToList();
            return availableTimeInMinutes;
        }



        public bool CheckIfUserHasAvailableSubscription(string userId, DateTime trainingDate, int gymId)
        {
            var actualSubscriptions = _repository.GetValidSubscriptionsByUserForSpecificGym(userId, gymId).ToList();
            foreach (var subscription in actualSubscriptions)
            {
                if (subscription.StartDate <= trainingDate && 
                    subscription.EndDate >= trainingDate)
                {
                    return true;
                }
            }

            return false;
        }

        

        public bool AddEvent(EventModel newEvent)
        {
            var eventEntityBase = _mapper.MapEventModelToEntityBase(newEvent);
            bool result = _repository.AddEvent(eventEntityBase);
            return result;
        }
    }
}
