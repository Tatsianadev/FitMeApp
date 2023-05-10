using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services
{
    public sealed class TrainingService : ITrainingService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;

        public TrainingService(IRepository repository)
        {
            _repository = repository;
            _mapper = new EntityModelMapper();
        }



        public ICollection<TrainingModel> GetAllTrainingModels()
        {
            var trainings = _repository.GetAllTrainings();
            var trainingModels = new List<TrainingModel>();

            foreach (var groupClass in trainings)
            {
                trainingModels.Add(_mapper.MapTrainingEntityBaseToModelBase(groupClass));
            }
            return trainingModels;
        }


        public TrainingModel GetTrainingModel(int trainingId)//todo delete comment
        {
            //var trainingEntity = _repository.GetTraining(trainingId);
            //TrainingModel trainingModel = _mapper.MapTrainingEntityBaseToModelBase(trainingEntity);
            var trainingEntity = _repository.GetTrainingWithTrainersAndGyms(trainingId);
            TrainingModel trainingModel = _mapper.MapTrainingWithTrainersAndGymsBaseToModel(trainingEntity);
            return trainingModel;
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


        //GroupClasses
        public IEnumerable<GroupClassScheduleModel> GetSpecificGroupClassSchedule(int groupClassId, string trainerId)
        {
            var groupClassScheduleModels = new List<GroupClassScheduleModel>();

            var groupClassScheduleEntities = _repository.GetSpecificGroupClassSchedule(groupClassId, trainerId);
            foreach (var entity in groupClassScheduleEntities)
            {
                int participantsCount = _repository.GetGroupClassParticipantsCount(entity.Id);
                groupClassScheduleModels.Add(new GroupClassScheduleModel()
                {
                    Date = entity.Date,
                    StartTime = entity.StartTime,
                    EndTime = entity.EndTime,
                    ParticipantsLimit = entity.ParticipantsLimit,
                    ActualParticipantsCount = participantsCount
                });
            }

            return groupClassScheduleModels;

        }
    }
}
