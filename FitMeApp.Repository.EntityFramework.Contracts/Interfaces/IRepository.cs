﻿using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
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

        //Trainers
        IEnumerable<TrainerEntityBase> GetAllTrainers();
        TrainerEntityBase GetTrainer(string id);
        TrainerEntityBase AddTrainer(TrainerEntityBase trainer);
        bool UpdateTrainer(string id, TrainerEntityBase newTrainerData);
        bool DeleteTrainer(string id);

        //Trainings
        IEnumerable<TrainingEntityBase> GetAllTrainings();
        TrainingEntityBase GetTraining(int id);
        TrainingEntityBase AddTraining(TrainingEntityBase item);
        bool UpdateTraining(int id, TrainingEntityBase newTrainingData);
        bool DeleteTraining(int id);

        //Gym - Trainer - Training connection
        GymWithTrainersAndTrainings GetGymWithTrainersAndTrainings(int gymId);
        List<TrainerWithGymAndTrainingsBase> GetAllTrainersWithGymAndTrainings();
        TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(string trainerId);
        TrainingWithTrainerAndGymBase GetTrainingWithTrainerAndGym(int trainingId);

        //Filters
        IEnumerable<GymEntityBase> GetGymsByTrainings(List<int> trainingsId);
        IEnumerable<SubscriptionPriceBase> GetSubscriptionsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring);

        //Subscriptions
        IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsByGym(int gymId);
        List<int> GetAllSubscriptionPeriods();
        int GetSubscriptionPeriod(int subscriptionId);
        SubscriptionPriceBase GetSubscriptionByGym(int subscriptionId, int gymId);

        //UserSubscriptions
        bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate);

        //Schedule
        IEnumerable<EventEntityBase> GetAllEvents();
        IEnumerable<EventEntityBase> GetEventsByUser(string userId);
        IEnumerable<EventWithNamesBase> GetEventsByUserAndDate(string userId, DateTime dateTime);
        IEnumerable<EventWithNamesBase> GetEventsByTrainerAndDate(string trainerId, DateTime date);
        IDictionary<string, int> GetEventsCountForEachDateByUser(string userId);
        IDictionary<string, int> GetEventsCountForEachDateByTrainer(string trainerId);


    }
}
