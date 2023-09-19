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
        TrainerModel GetTrainerWithGymAndTrainings(string trainerId);
        int GetGymIdByTrainer(string trainerId);
        IEnumerable<TrainerWorkHoursModel> GetWorkHoursByTrainer(string trainerId);
        void UpdateTrainingsSetByTrainer(string trainerId, List<int> newTrainingsSet);
        void UpdateTrainerSpecialization(string trainerId, TrainerSpecializationsEnum newSpecialization);
        bool TryUpdateTrainerWorkHours(List<TrainerWorkHoursModel> newWorkHours);
        IEnumerable<string> GetAllClientsIdByTrainer(string trainerId);
        IEnumerable<string> GetActualClientsIdByTrainer(string trainerId);
        IEnumerable<TrainerModel> GetTrainersByFilter(List<GenderEnum> selectedGenders, List<TrainerSpecializationsEnum> selectedSpecializations);
        void DeleteTrainer(string id);
        void DeleteTrainerWorkHoursByTrainer(string trainerId);
        bool AddTrainer(TrainerModel trainer);
        string GetTrainerSpecialization(string trainerId);


        //Training-Trainer Connection
        void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId);
        bool AddTrainingTrainerConnection(string trainerId, int trainingId);
        TrainerSpecializationsEnum GetTrainerSpecializationByTrainings(IEnumerable<int> trainingsId);

        //TrainerApplication
        int AddTrainerApplication(TrainerApplicationModel trainerApplication);
        int GetTrainerApplicationsCount();
        IEnumerable<TrainerApplicationModel> GetAllTrainerApplications();
        bool ApproveTrainerApplication(string userId);
        TrainerApplicationModel GetTrainerApplicationByUser(string userId);
        void DeleteTrainerApplication(int applicationId);

        //TrainerLicense

        TrainerWorkLicenseModel GetTrainerWorkLicenseByTrainer(string userId);
        IEnumerable<TrainerWorkLicenseModel> GetAllTrainerWorkLicenses();
        void ReplaceTrainerWorkLicense(string userId, TrainerWorkLicenseModel newLicense);



    }
}
