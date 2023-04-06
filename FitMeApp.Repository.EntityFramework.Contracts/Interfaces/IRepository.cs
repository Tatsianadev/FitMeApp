using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities.JoinEntityBase;
using Microsoft.AspNetCore.Identity;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;

namespace FitMeApp.Repository.EntityFramework.Contracts.Interfaces
{
    public interface IRepository
    {
        //Gym
        IEnumerable<GymEntityBase> GetAllGyms();
        IEnumerable<GymWithGalleryBase> GetAllGymsWithGallery();
        GymEntityBase GetGym(int id);
        GymEntityBase AddGym(GymEntityBase item);
        void UpdateGym(int id, GymEntityBase newGymData);
        void DeleteGym(int id);
        IEnumerable<GymWorkHoursEntityBase> GetWorkHoursByGym(int gymId);
        int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek);

        //Trainers
        IEnumerable<TrainerEntityBase> GetAllTrainers();
        TrainerEntityBase GetTrainer(string id);
        bool AddTrainer(TrainerEntityBase trainer);
        void UpdateTrainer(TrainerEntityBase newTrainerData);
        void DeleteTrainer(string id);
        int GetGymIdByTrainer(string trainerId);
        IEnumerable<TrainerWorkHoursWithDayBase> GetWorkHoursByTrainer(string trainerId);
        void AddTrainerWorkHours(TrainerWorkHoursEntityBase workHoursBase);
        void DeleteTrainerWorkHours(int workHoursId);
        void UpdateTrainerWorkHours(TrainerWorkHoursEntityBase newTrainerWorkHours);
        IEnumerable<int> GerAllTrainerWorkHoursId(string trainerId);
        IEnumerable<string> GetAllClientsIdByTrainer(string trainerId);
        IEnumerable<string> GetActualClientsIdByTrainer(string trainerId);
        void DeleteTrainerWorkHoursByTrainer(string trainerId);
        IEnumerable<int> GetAvailableToApplyTrainingTimeByTrainer(string trainerId, DateTime date);
        int AddTrainerWorkLicense(TrainerWorkLicenseEntityBase license);
        void DeleteTAllTrainerWorkLicensesByTrainer(string trainerId);
        TrainerWorkLicenseEntityBase GetTrainerWorkLicense(int licenseId);
        



        //TrainerApplication
        IEnumerable<TrainerApplicationWithNamesBase> GetAllTrainerApplications();
        TrainerApplicationWithNamesBase GetTrainerApplicationWithNamesByUser(string userId);
        TrainerApplicationEntityBase GetTrainerApplicationByUser(string userId);
        int AddTrainerApplication(TrainerApplicationEntityBase trainerApplication);
        int GetTrainerApplicationsCount();
        void DeleteTrainerApplication(int appId);


        //Trainings
        IEnumerable<TrainingEntityBase> GetAllTrainings();
        TrainingEntityBase GetTraining(int id);
        TrainingEntityBase AddTraining(TrainingEntityBase training);
        void UpdateTraining(int id, TrainingEntityBase newTrainingData);
        void DeleteTraining(int id);


        //Trainer-Training 
        IEnumerable<int> GetAllTrainingIdsByTrainer(string trainerId);
        void DeleteTrainingTrainerConnection(string trainerId, int trainingToDeleteId);
        bool AddTrainingTrainerConnection(string trainerId, int trainingToAddId);
        void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId);

        //Gym - Trainer - Training connection
        GymWithTrainersAndTrainings GetGymWithTrainersAndTrainings(int gymId);
        IEnumerable<TrainerWithGymAndTrainingsBase> GetAllTrainersWithGymAndTrainings();
        TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(string trainerId);

        //Filters
        IEnumerable<GymWithGalleryBase> GetGymsByTrainings(IEnumerable<int> trainingsId);
        IEnumerable<SubscriptionPriceBase> GetSubscriptionsForVisitorsByGymByFilter(int gymId, IEnumerable<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<TrainerWithGymAndTrainingsBase> GetTrainersWithGymAndTrainingsByFilter(IEnumerable<string> selectedGenders, IEnumerable<string> selectedSpecializations);

        //Subscriptions
        IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsForVisitorsByGym(int gymId);
        IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsForTrainersByGym(int gymId);
        IEnumerable<int> GetAllSubscriptionPeriods();
        int GetSubscriptionPeriod(int subscriptionId);
        SubscriptionPriceBase GetSubscriptionWithPriceByGym(int subscriptionId, int gymId);

        //UserSubscriptions
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        IEnumerable<UserSubscriptionFullInfoBase> GetUserSubscriptionsFullInfo(string userId);
        IEnumerable<UserSubscriptionEntityBase> GetValidSubscriptionsByUserForSpecificGym(string userId, int gymId);
        IEnumerable<UserSubscriptionFullInfoBase> GetValidSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds);
        IEnumerable<UserSubscriptionFullInfoBase> GetExpiredSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds);
        IEnumerable<UserSubscriptionFullInfoBase> GetValidInTheFutureSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds);


        //Schedule
        IEnumerable<EventEntityBase> GetAllEvents();
        IEnumerable<EventEntityBase> GetEventsByUser(string userId);
        IEnumerable<EventWithNamesBase> GetEventsByUserAndDate(string userId, DateTime dateTime);
        IEnumerable<EventWithNamesBase> GetEventsByTrainerAndDate(string trainerId, DateTime date);
        IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId);
        IDictionary<DateTime, int> GetEventsCountForEachDateByTrainer(string trainerId);

        //Events
        void ChangeEventStatus(int eventId);
        int GetActualEventsCountByTrainer(string trainerId);
        IEnumerable<EventEntityBase> GetActualEventsByTrainer(string trainerId);
        bool AddEvent(EventEntityBase newEvent);

        //Chat
        IEnumerable<ChatMessageEntityBase> GetAllMessagesByUser(string userId);
        IEnumerable<ChatMessageEntityBase> GetAllMessagesBetweenTwoUsers(string senderId, string receiverId);
        IEnumerable<string> GetAllContactsIdByUser(string userId);
        int AddMessage(ChatMessageEntityBase message);
        ChatMessageEntityBase GetMessage(int messageId);
        void AddContact(string userId, string interlocutorId);

        //Chart/diagrams

        void DeleteNumberOfVisitorsPerHourChartData(int gymId);
        void AddNumberOfVisitorsPerHourChartData(IEnumerable<NumberOfVisitorsPerHourEntityBase> chartData);
        IEnumerable<NumberOfVisitorsPerHourEntityBase> GetAllNumberOfVisitorsPerHourByGym(int gymId);





    }
}
