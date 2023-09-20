using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ITrainingService
    {
        ICollection<TrainingModel> GetAllTrainingModels();
        TrainingModel GetTrainingModel(int trainingId, int gymId = 0);
        IEnumerable<int> GetAvailableTimeForTraining (string trainerId, DateTime date);
        bool CheckIfUserHasAvailableSubscription(string userId, DateTime trainingDate, int gymId);
        int GetPrice(string trainerId);
        int UpdatePersonalTrainingPrice(string trainerId, int newPricePerHour);
        IEnumerable<int> GetTrainingIdsOfActualEventsForTrainer(string trainerId);

        //GroupClasses
        IEnumerable<GroupClassScheduleRecordModel> GetAllRecordsInGroupClassScheduleByClassAndTrainer(int groupClassId, string trainerId);
        GroupClassScheduleRecordModel GetRecordInGroupClassSchedule(int groupClassScheduleId);
        IEnumerable<GroupClassScheduleRecordModel> GetAllRecordsInGroupClassScheduleByTrainerAndDate(string trainerId, DateTime date);
        int GetGroupClassScheduleRecordId(string trainerId, int trainingId, DateTime date, int startTime);
        int AddGroupClassParticipant(int groupClassScheduleId, string userId);
        int GetGroupClassScheduleRecordsCount(string trainerId, List<DateTime> dates, int startTime, int endTime);
        IEnumerable<int> AddGroupClassScheduleRecords(List<GroupClassScheduleRecordModel> groupClassScheduleModels);
        IEnumerable<string> GetAllParticipantIdsByGroupClass(int groupClassScheduleRecordId);
        void DeleteGroupClassScheduleRecord(int grClassScheduleRecordId, int actualParticipantsCount);
        void DeleteParticipant(string userId, int groupClassScheduleRecordId);


    }
}
