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
        IEnumerable<GroupClassScheduleModel> GetSpecificGroupClassSchedule(int groupClassId, string trainerId);
        GroupClassScheduleModel GetRecordInGroupClassSchedule(int groupClassScheduleId);
        IEnumerable<GroupClassScheduleModel> GetAllGroupClassesScheduleByTrainer(string trainerId);
        int AddGroupClassParticipant(int groupClassScheduleId, string userId);
       



    }
}
