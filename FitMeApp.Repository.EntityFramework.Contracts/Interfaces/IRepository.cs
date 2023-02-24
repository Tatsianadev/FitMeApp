using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace FitMeApp.Repository.EntityFramework.Contracts.Interfaces
{
    public interface IRepository
    {
        //Gym
        IEnumerable<GymEntityBase> GetAllGyms();
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
        IEnumerable<TrainerWorkHoursWithDayBase> GetWorkHoursByTrainer(string trainerId);       
        bool AddTrainerWorkHours(TrainerWorkHoursEntityBase workHoursBase);
        void DeleteTrainerWorkHours(int workHoursId);
        void UpdateTrainerWorkHours(TrainerWorkHoursEntityBase newTrainerWorkHours);
        IEnumerable<int> GerAllTrainerWorkHoursId(string trainerId);
        IEnumerable<string> GetAllClientsIdByTrainer(string trainerId);
        void DeleteTrainerWorkHoursByTrainer(string trainerId);
        IEnumerable<int> GetAvailableToApplyTrainingTimeByTrainer(string trainerId, DateTime date);

        //Trainings
        IEnumerable<TrainingEntityBase> GetAllTrainings();
        IEnumerable<TrainerWithGymAndTrainingsBase> GetAllTrainersByStatus(TrainerApproveStatusEnum status);
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
        List<TrainerWithGymAndTrainingsBase> GetAllTrainersWithGymAndTrainings();       
        TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(string trainerId);
        
        //Filters
        IEnumerable<GymEntityBase> GetGymsByTrainings(List<int> trainingsId);
        IEnumerable<SubscriptionPriceBase> GetSubscriptionsForVisitorsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);
        IEnumerable<TrainerWithGymAndTrainingsBase> GetTrainersWithGymAndTrainingsByFilter(List<string> selectedGenders, List<string> selectedSpecializations);

        //Subscriptions
        IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsForVisitorsByGym(int gymId);
        IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsForTrainersByGym(int gymId);
        List<int> GetAllSubscriptionPeriods();
        int GetSubscriptionPeriod(int subscriptionId);
        SubscriptionPriceBase GetSubscriptionWithPriceByGym(int subscriptionId, int gymId);

        //UserSubscriptions
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        //int GetActualSubscriptionsCountByTrainer(string trainerId);
        IEnumerable<UserSubscriptionFullInfoBase> GetUserSubscriptionsFullInfo(string userId);
        IEnumerable<UserSubscriptionEntityBase> GetValidSubscriptionsByUserForSpecificGym(string userId, int gymId);
        IEnumerable<UserSubscriptionFullInfoBase> GetValidSubscriptionsByUserForGyms(string userId, List<int> gymIds);
        IEnumerable<UserSubscriptionFullInfoBase> GetExpiredSubscriptionsByUserForGyms(string userId, List<int> gymIds);
        IEnumerable<UserSubscriptionFullInfoBase> GetValidInTheFutureSubscriptionsByUserForGyms(string userId,
            List<int> gymIds);


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
        bool AddEvent(EventEntityBase newEvent);

        //Chat
        IEnumerable<ChatMessageEntityBase> GetAllMessagesByUser(string userId);
        IEnumerable<ChatMessageEntityBase> GetAllMessagesBetweenTwoUsers(string senderId, string receiverId);
        IEnumerable<string> GetAllContactsIdByUser(string userId);
        int AddMessage(ChatMessageEntityBase message);
        ChatMessageEntityBase GetMessage(int messageId);
        bool AddContact(string userId, string interlocutorId);

        //Users - Roles
        //IEnumerable<User> GetUsersByRoles(IEnumerable<IdentityRole> roles);


        //Test
        void AddAvatarFile(AvatarsTestEntityBase avatarFile);
        IEnumerable<AvatarsTestEntityBase> GetAllAvatars();
    }
}
