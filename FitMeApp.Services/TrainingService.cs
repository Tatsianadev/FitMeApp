using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;
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


        public TrainingModel GetTrainingModel(int trainingId, int gymId = 0)
        {
            var trainingEntity = _repository.GetTrainingWithTrainersAndGyms(trainingId);
            if (gymId != 0)
            {
                trainingEntity.Trainers = trainingEntity.Trainers.Where(x => x.Gym.Id == gymId);
            }
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
            var actualSubscriptions = _repository.GetValidSubscriptionsByUserForSpecificGym(userId, gymId, trainingDate).ToList();
           return actualSubscriptions.Count > 0;
        }


        public int GetPrice(string trainerId)
        {
            var price = _repository.GetPrice(trainerId);
            return price;
        }

        public int UpdatePersonalTrainingPrice(string trainerId, int newPricePerHour)
        {
            int personalTrainingPriceId = _repository.UpdatePersonalTrainingPrice(trainerId, newPricePerHour);
            return personalTrainingPriceId;
        }



        //GroupClasses

        //List of all groupClass records for schedule Calendar
        public IEnumerable<GroupClassScheduleRecordModel> GetAllRecordsInGroupClassScheduleByClassAndTrainer(int groupClassId, string trainerId)
        {
            var groupClassScheduleModels = new List<GroupClassScheduleRecordModel>();

            var groupClassScheduleEntities = _repository.GetAllRecordsInGroupClassScheduleByClassAndTrainer(groupClassId, trainerId);
            foreach (var entity in groupClassScheduleEntities)
            {
                int participantsCount = _repository.GetGroupClassParticipantsCount(entity.Id);
                groupClassScheduleModels.Add(new GroupClassScheduleRecordModel()
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
        public GroupClassScheduleRecordModel GetRecordInGroupClassSchedule(int groupClassScheduleId)
        {
            var entity = _repository.GetRecordInGroupTrainingSchedule(groupClassScheduleId);
            return GetGroupClassScheduleRecordModelByEntity(entity);
        }

        

        public IEnumerable<GroupClassScheduleRecordModel> GetAllRecordsInGroupClassScheduleByTrainerAndDate(string trainerId,
            DateTime date)
        {
            var groupClassScheduleRecordEntities = _repository.GetAllRecordsInGroupClassScheduleByTrainerAndDate(trainerId, date);
            var groupClassScheduleRecordModels = new List<GroupClassScheduleRecordModel>(); 
            foreach (var entity in groupClassScheduleRecordEntities)
            {
                var groupClassEventModel = GetGroupClassScheduleRecordModelByEntity(entity);
                groupClassScheduleRecordModels.Add(groupClassEventModel);
            }

            return groupClassScheduleRecordModels;
        }


        public int AddGroupClassParticipant(int groupClassScheduleId, string userId)
        {
            int participantId = _repository.AddGroupClassParticipant(groupClassScheduleId, userId);
            return participantId;
        }


        public int GetGroupClassScheduleRecordsCount(string trainerId, List<DateTime> dates, int startTime, int endTime)
        {
            int groupClassRecordsCount = _repository.GetGroupClassScheduleRecordsCount(trainerId, dates, startTime, endTime);
            return groupClassRecordsCount;
        }


        public IEnumerable<int> AddGroupClassScheduleRecords(List<GroupClassScheduleRecordModel> groupClassScheduleModels)
        {
            var groupClassScheduleRecordsIds = new List<int>();

            if (groupClassScheduleModels.Count > 1)
            {
                var groupClassScheduleRecordsEntityBase = new List<GroupClassScheduleRecordEntityBase>();
                foreach (var model in groupClassScheduleModels)
                {
                    groupClassScheduleRecordsEntityBase.Add(GetGroupClassScheduleEntityBaseByModel(model));
                }

                groupClassScheduleRecordsIds = _repository.AddRangeGroupClassScheduleRecords(groupClassScheduleRecordsEntityBase).ToList();
            }
            else
            {
                var groupClassScheduleRecordEntityBase = GetGroupClassScheduleEntityBaseByModel(groupClassScheduleModels.First());
                groupClassScheduleRecordsIds.Add(_repository.AddGroupClassScheduleRecord(groupClassScheduleRecordEntityBase));
            }

            return groupClassScheduleRecordsIds;
        }


        public IEnumerable<string> GetAllParticipantIdsByGroupClass(int groupClassScheduleRecordId)
        {
            var participantIds = _repository.GetAllParticipantIdsByGroupClass(groupClassScheduleRecordId);
            return participantIds;
        }


        public void DeleteGroupClassScheduleRecord(int grClassScheduleRecordId, int actualParticipantsCount)
        {
            if (actualParticipantsCount > 0)
            {
                _repository.DeleteGroupClassEventForAllParticipants(grClassScheduleRecordId);
                _repository.DeleteParticipants(grClassScheduleRecordId);
            }

            _repository.DeleteGroupClassScheduleRecord(grClassScheduleRecordId);
        }



        private GroupClassScheduleRecordModel GetGroupClassScheduleRecordModelByEntity(GroupClassScheduleRecordFullInfo entity)
        {
            var participantsCount = _repository.GetGroupClassParticipantsCount(entity.Id);

            var grClassScheduleRecord = new GroupClassScheduleRecordModel()
            {
                Id = entity.Id,
                TrainerId = entity.TrainerId,
                GymId = entity.GymId,
                GroupClassId = entity.GroupClassId,
                GroupClassName = entity.GroupClassName,
                Date = entity.Date,
                StartTime = entity.StartTime,
                EndTime = entity.EndTime,
                ParticipantsLimit = entity.ParticipantsLimit,
                ActualParticipantsCount = participantsCount
            };

            return grClassScheduleRecord;
        }


        private GroupClassScheduleRecordEntityBase GetGroupClassScheduleEntityBaseByModel(GroupClassScheduleRecordModel model)
        {
            int trainerTrainingId = _repository.GetTrainingTrainerConnectionId(model.TrainerId, model.GroupClassId);

            var entityBase = new GroupClassScheduleRecordEntityBase()
            {
                Id = model.Id,
                TrainingTrainerId = trainerTrainingId,
                GymId = model.GymId,
                Date = model.Date,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                ParticipantsLimit = model.ParticipantsLimit
            };

            return entityBase;
        }
    }
}
