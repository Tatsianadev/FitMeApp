using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;


namespace FitMeApp.Repository.EntityFramework.Contracts.Interfaces
{
    public interface IRepository
    {
        //Gym
        IEnumerable<GymEntityBase> GetAllGyms();
        GymEntityBase GetGym(int id);
        GymEntityBase AddGym(GymEntityBase item);
        bool UpdateGym(int id, GymEntityBase newGymData);
        bool DeleteGym(int id);
        IEnumerable<GymWorkHoursEntityBase> GetWorkHoursByGym(int gymId);
        int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek);
        

        //Trainers
        IEnumerable<TrainerEntityBase> GetAllTrainers();        
        TrainerEntityBase GetTrainer(string id);
        bool AddTrainer(TrainerEntityBase trainer);
        void UpdateTrainer(TrainerEntityBase newTrainerData);
        bool DeleteTrainer(string id);
        IEnumerable<TrainerWorkHoursWithDayBase> GetWorkHoursByTrainer(string trainerId);       
        bool AddTrainerWorkHours(TrainerWorkHoursEntityBase workHoursBase);
        bool DeleteTrainerWorkHours(int workHoursId);
        bool UpdateTrainerWorkHours(TrainerWorkHoursEntityBase newTrainerWorkHours);
        IEnumerable<int> GerAllTrainerWorkHoursId(string trainerId);
        IEnumerable<string> GetAllClientsIdByTrainer(string trainerId);
        bool DeleteTrainerWorkHoursByTrainer(string trainerId);

       



        //Trainings
        IEnumerable<TrainingEntityBase> GetAllTrainings();
        IEnumerable<TrainerWithGymAndTrainingsBase> GetAllTrainersWithNames();
        TrainingEntityBase GetTraining(int id);
        TrainingEntityBase AddTraining(TrainingEntityBase item);
        bool UpdateTraining(int id, TrainingEntityBase newTrainingData);
        bool DeleteTraining(int id);

        //Trainer-Training 
        bool DeleteTrainingTrainerConnection(string trainerId, int trainingToDeleteId);
        bool AddTrainingTrainerConnection(string trainerId, int trainingToAddId);
        bool DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId);


        //Gym - Trainer - Training connection
        GymWithTrainersAndTrainings GetGymWithTrainersAndTrainings(int gymId);
        List<TrainerWithGymAndTrainingsBase> GetAllTrainersWithGymAndTrainings();       
        TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(string trainerId);
        bool UpdateTrainerWithGymAndTrainings(TrainerWithGymAndTrainingsBase newTrainerInfo);
        TrainingWithTrainerAndGymBase GetTrainingWithTrainerAndGym(int trainingId);

        //Filters
        IEnumerable<GymEntityBase> GetGymsByTrainings(List<int> trainingsId);
        IEnumerable<SubscriptionPriceBase> GetSubscriptionsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<TrainerWithGymAndTrainingsBase> GetTrainersWithGymAndTrainengsByFilter(List<string> selectedGenders, List<string> selectedSpecializations);

        //Subscriptions
        IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsByGym(int gymId);
        List<int> GetAllSubscriptionPeriods();
        int GetSubscriptionPeriod(int subscriptionId);
        SubscriptionPriceBase GetSubscriptionWithPriceByGym(int subscriptionId, int gymId);

        //UserSubscriptions
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        int GetActualSubscriptionsCountByTrainer(string trainerId);
        IEnumerable<UserSubscriptionWithIncludedOptionsBase> GetUserSubscriptionsFullInfo(string userId);


       

       //Schedule
       IEnumerable<EventEntityBase> GetAllEvents();
        IEnumerable<EventEntityBase> GetEventsByUser(string userId);
        IEnumerable<EventWithNamesBase> GetEventsByUserAndDate(string userId, DateTime dateTime);
        IEnumerable<EventWithNamesBase> GetEventsByTrainerAndDate(string trainerId, DateTime date);
        IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId);
        IDictionary<DateTime, int> GetEventsCountForEachDateByTrainer(string trainerId);

        //Events
        bool ChangeEventStatus(int eventId);
        int GetActualEventsCountByTrainer(string trainerId);
        IEnumerable<EventEntityBase> GetActualEventsByTrainer(string trainerId);



    }
}
