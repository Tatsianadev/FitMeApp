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
        void UpdateTrainerSpecialization(string trainerId, TrainerSpecializationsEnum newSpecialization);
        IEnumerable<int> GerAllTrainerWorkHoursId(string trainerId);
        IEnumerable<string> GetAllClientsIdByTrainer(string trainerId);
        IEnumerable<string> GetActualClientsIdByTrainer(string trainerId);
        void DeleteTrainerWorkHoursByTrainer(string trainerId);
        IEnumerable<int> GetAvailableToApplyTrainingTimeByTrainer(string trainerId, DateTime date);
        int AddTrainerWorkLicense(TrainerWorkLicenseEntityBase license);
        void DeleteAllTrainerWorkLicensesByTrainer(string trainerId);
        void DeleteTrainerWorkLicense(int licenseId);
        TrainerWorkLicenseEntityBase GetTrainerWorkLicense(int licenseId);
        TrainerWorkLicenseEntityBase GetTrainerWorkLicenseByTrainer(string trainerId);
        IEnumerable<TrainerWorkLicenseEntityBase> GetAllTrainerWorkLicense();
        int GetPrice(string trainerId);
        int UpdatePersonalTrainingPrice(string trainerId, int newPricePerHour);



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

        //GroupClasses
        IEnumerable<GroupClassScheduleRecordEntityBase> GetAllRecordsInGroupClassScheduleByClassAndTrainer(int groupClassId, string trainerId);
        int GetGroupClassParticipantsCount(int groupClassScheduleId);
        GroupClassScheduleRecordFullInfo GetRecordInGroupTrainingSchedule(int groupTrainingScheduleId);
        int AddGroupClassParticipant(int groupClassScheduleId, string userId);
        IEnumerable<GroupClassScheduleRecordFullInfo> GetAllRecordsInGroupClassScheduleByTrainerAndDate(string trainerId, DateTime date);
        int GetGroupClassScheduleRecordId(int trainerTrainingId, DateTime date, int startTime);
        int GetGroupClassScheduleRecordsCount(string trainerId, List<DateTime> dates, int startTime, int endTime);
        int AddGroupClassScheduleRecord(GroupClassScheduleRecordEntityBase groupClassScheduleRecord);
        IEnumerable<int> AddRangeGroupClassScheduleRecords(List<GroupClassScheduleRecordEntityBase> groupClassScheduleRecords);
        void DeleteGroupClassScheduleRecord(int grClassScheduleRecordId);
        void DeleteParticipant(string userId, int groupClassScheduleRecordId);

        //Trainer-Training 
        IEnumerable<int> GetAllTrainingIdsByTrainer(string trainerId);
        void DeleteTrainingTrainerConnection(string trainerId, int trainingToDeleteId);
        bool AddTrainingTrainerConnection(string trainerId, int trainingToAddId);
        void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId);
        int GetTrainingTrainerConnectionId(string trainerId, int trainingId);

        //Gym - Trainer - Training connection
        GymWithTrainersAndTrainings GetGymWithTrainersAndTrainings(int gymId);
        IEnumerable<TrainerWithGymAndTrainingsBase> GetAllTrainersWithGymAndTrainings();
        TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(string trainerId);
        TrainingWithTrainerAndGymBase GetTrainingWithTrainersAndGyms(int trainingId);

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
        void DeleteUserSubscription(int userSubscriptionId);

        //UserSubscriptions
        int AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);
        IEnumerable<UserSubscriptionFullInfoBase> GetUserSubscriptionsFullInfo(string userId);
        IEnumerable<UserSubscriptionEntityBase> GetValidSubscriptionsByUserForSpecificGym(string userId, int gymId, DateTime dataToCheck);
        IEnumerable<UserSubscriptionFullInfoBase> GetValidNowSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds);
        IEnumerable<UserSubscriptionFullInfoBase> GetExpiredSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds);
        IEnumerable<UserSubscriptionFullInfoBase> GetValidInTheFutureSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds);
        void UpdateUserSubscriptionDates(int subscriptionId, DateTime startDate, DateTime endDate);
        IEnumerable<UserSubscriptionFullInfoBase> GetAllUsersByExpiringSubscriptions(int daysBeforeSubscrExpire);

        //Schedule
        IEnumerable<EventEntityBase> GetAllEvents();
        IEnumerable<EventEntityBase> GetEventsByUser(string userId);
        IEnumerable<EventEntityBase> GetOpenedEventsByTrainer(string trainerId);
        IEnumerable<EventFullInfoBase> GetEventsByUserAndDate(string userId, DateTime dateTime);
        IEnumerable<EventFullInfoBase> GetPersonalTrainingsByTrainerAndDate(string trainerId, DateTime date);
        IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId);
        IDictionary<DateTime, int> GetEventsCountForEachDateByTrainer(string trainerId);
        int GetEventsCount(string trainerId, List<DateTime> dates, int startTime, int endTime);
        IEnumerable<string> GetAllParticipantIdsByGroupClass(int groupClassScheduleRecordId);
        void DeleteParticipants(int grClassScheduleRecordId);
        void DeleteGroupClassEventForAllParticipants(int grClassScheduleRecordId);


        //Events
        void ConfirmEvent(int eventId);
        EventEntityBase GetEvent(int eventId);
        int GetActualEventsCountByTrainer(string trainerId);
        IEnumerable<int> GetTrainingIdsOfActualEventsForTrainer(string trainerId);
        IEnumerable<EventEntityBase> GetActualEventsByTrainer(string trainerId);
        int AddEvent(EventEntityBase newEvent);
        void DeleteEvent(int eventId);

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
        IEnumerable<NumberOfVisitorsPerHourEntityBase> GetNumOfVisitorsPerHourOnCertainDayByGym(int gymId, DayOfWeek day);

        //Diet
        int AddAnthropometricInfo(AnthropometricInfoEntityBase info);
        int AddDiet(DietEntityBase diet, string userId);
        AnthropometricInfoEntityBase GetLatestAnthropometricInfo(string userId);
        IEnumerable<AnthropometricInfoEntityBase> GetAllAnthropometricInfoByUser(string userId);
        DietEntityBase GetDiet(int anthropometricInfoId);
        DietEntityBase GetDietByUser(string userId);





    }
}
