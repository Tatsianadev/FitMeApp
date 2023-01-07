using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using System;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFitMeService
    {
        //Gyms
        IEnumerable<GymModel> GetAllGymModels();
        GymModel GetGymModel(int id);
        IEnumerable<GymModel> GetGymsByTrainings(List<int> groupClassesId);
        IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId);
        int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek);
        int GetGymIdByTrainer(string trainerId);    

        //Trainings
        ICollection<TrainingModel> GetAllTrainingModels();
        TrainingModel GetTrainingModel(int trainingId);
       

        //Subscriptions
        IEnumerable<SubscriptionModel> GetSubscriptionsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<SubscriptionModel> GetAllSubscriptionsByGym(int gymId);
        List<int> GetAllSubscriptionPeriods();

        SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId);

        //UserSubscriptions
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        int GetActualSubscriptionsCountByTrainer(string trainerId);
        IEnumerable<UserSubscriptionModel> GetUserSubscriptions(string userId);

     

        //Trainers
        List<TrainerModel> GetAllTrainerModels();
        IEnumerable<TrainerModel> GetAllTrainersNames();
        bool UpdateTrainerWithGymAndTrainings(TrainerModel newTrainerInfo);
        TrainerModel GetTrainerWithGymAndTrainings(string trainerId);
        IEnumerable<TrainerWorkHoursModel> GetWorkHoursByTrainer(string trainerId);
        bool CheckFacilityUpdateTrainerWorkHoursByGymScedule(int gymId, List<TrainerWorkHoursModel> newWorkHours);
        bool CheckFacilityUpdateTrainerWorkHoursByEvents(List<TrainerWorkHoursModel> newWorkHours);
        bool CheckFacilityUpdateTrainerWorkHours(List<TrainerWorkHoursModel> newWorkHours);
        bool UpdateTrainerWorkHours(List<TrainerWorkHoursModel> trainerWorkHours);
        IEnumerable<string> GetAllClientsIdByTrainer(string trainerId);
        IEnumerable<TrainerModel> GetTrainersByFilter(List<GenderEnum> selectedGenders, List<TrainerSpecializationsEnum> selectedSpecializations);
        bool DeleteTrainer(string id);
        bool DeleteTrainerWorkHoursByTrainer(string trainerId);
        bool AddTrainer(TrainerModel trainer);

        //Events
        int GetActualEventsCountByTrainer(string trainerId);


        //TrainingTrainer 
        bool DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId);
        bool AddTrainingTrainerConnection(string trainerId, int trainingId);


    }
}
