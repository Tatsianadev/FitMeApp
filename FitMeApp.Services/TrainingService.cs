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


        //public int AddEvent(EventModel newEvent)
        //{
        //    var eventEntityBase = _mapper.MapEventModelToEntityBase(newEvent);
        //    int eventId = _repository.AddEvent(eventEntityBase);
        //    return eventId;
        //}


        //public void DeleteEvent(int eventId)
        //{
        //   _repository.DeleteEvent(eventId);
        //}


        //GroupClasses

        //List of all groupClass records for schedule Calendar
        public IEnumerable<GroupClassScheduleModel> GetSpecificGroupClassSchedule(int groupClassId, string trainerId)
        {
            var groupClassScheduleModels = new List<GroupClassScheduleModel>();

            var groupClassScheduleEntities = _repository.GetScheduleForSelectedGroupClass(groupClassId, trainerId);
            foreach (var entity in groupClassScheduleEntities)
            {
                int participantsCount = _repository.GetGroupClassParticipantsCount(entity.Id);
                groupClassScheduleModels.Add(new GroupClassScheduleModel()
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    StartTime = entity.StartTime,
                    EndTime = entity.EndTime,
                    ParticipantsLimit = entity.ParticipantsLimit,
                    ActualParticipantsCount = participantsCount
                });
            }

            return groupClassScheduleModels;

        }


        //Full info about concrete groupClass record in Schedule Calendar
        public GroupClassScheduleModel GetRecordInGroupClassSchedule(int groupClassScheduleId)
        {
            var entity = _repository.GetRecordInGroupTrainingSchedule(groupClassScheduleId);
            var participantsCount = _repository.GetGroupClassParticipantsCount(entity.Id);

            var grClassScheduleRecord = new GroupClassScheduleModel()
            {
                Id = entity.Id,
                TrainerId = entity.TrainerId,
                GymId = entity.GymId,
                GroupClassId = entity.GroupClassId,
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ParticipantsLimit = entity.ParticipantsLimit,
                ActualParticipantsCount = participantsCount
            };
            
            return grClassScheduleRecord;
        }



        public int AddGroupClassParticipant(int groupClassScheduleId, string userId)
        {
            int participantId = _repository.AddGroupClassParticipant(groupClassScheduleId, userId);
            return participantId;
        }


    }
}
