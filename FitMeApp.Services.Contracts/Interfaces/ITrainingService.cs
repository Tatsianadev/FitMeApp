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
        TrainingModel GetTrainingModel(int trainingId);
        IEnumerable<int> GetAvailableTimeForTraining (string trainerId, DateTime date);
        bool CheckIfUserHasAvailableSubscription(string userId, DateTime trainingDate, int gymId);

        //GroupClasses
        IEnumerable<GroupClassScheduleRecordModel> GetAllRecordsInGroupClassScheduleByClassAndTrainer(int groupClassId, string trainerId);
        GroupClassScheduleRecordModel GetRecordInGroupClassSchedule(int groupClassScheduleId);
        IEnumerable<GroupClassScheduleRecordModel> GetAllRecordsInGroupClassScheduleByTrainerAndDate(string trainerId, DateTime date);
        int AddGroupClassParticipant(int groupClassScheduleId, string userId);
        int GetGroupClassScheduleRecordsCount(string trainerId, List<DateTime> dates, int startTime, int endTime);
        IEnumerable<int> AddGroupClassScheduleRecords(List<GroupClassScheduleRecordModel> groupClassScheduleModels);





    }
}
