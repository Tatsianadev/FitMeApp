using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ITrainerService
    {
        List<TrainerModel> GetAllTrainerModels();
        IEnumerable<TrainerModel> GetAllTrainersByStatus(TrainerApproveStatusEnum status);
        void UpdateTrainerWithGymAndTrainings(TrainerModel newTrainerInfo);
        void UpdateTrainerStatus(string trainerId, TrainerApproveStatusEnum newStatus);
        TrainerModel GetTrainerWithGymAndTrainings(string trainerId);
        IEnumerable<TrainerWorkHoursModel> GetWorkHoursByTrainer(string trainerId);
        bool CheckFacilityUpdateTrainerWorkHours(List<TrainerWorkHoursModel> newWorkHours);
        bool UpdateTrainerWorkHours(List<TrainerWorkHoursModel> trainerWorkHours);
        IEnumerable<string> GetAllClientsIdByTrainer(string trainerId);
        IEnumerable<string> GetActualClientsIdByTrainer(string trainerId);
        IEnumerable<TrainerModel> GetTrainersByFilter(List<GenderEnum> selectedGenders, List<TrainerSpecializationsEnum> selectedSpecializations);
        void DeleteTrainer(string id);
        void DeleteTrainerWorkHoursByTrainer(string trainerId);
        bool AddTrainer(TrainerModel trainer);

        //Training-Trainer Connection
        void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId);
        bool AddTrainingTrainerConnection(string trainerId, int trainingId);

        //TrainerApplication
        int AddTrainerApplication(TrainerApplicationModel trainerApplication);
    }
}
