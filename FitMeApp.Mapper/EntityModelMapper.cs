﻿using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


namespace FitMeApp.Mapper
{
    public class EntityModelMapper
    {

        //ModelBase - Model with properties as EntityBase classes have. Without references to other classes (foreign key connections). 
        public GymModel MappGymEntityBaseToModelBase(GymEntityBase gym)
        {
            GymModel gymModel = new GymModel()
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Phone = gym.Phone
            };
            return gymModel;
        }

        public TrainerModel MappTrainerEntityBaseToModelBase(TrainerWithGymAndTrainingsBase trainer)
        {
            var trainings = new List<TrainingModel>();
            foreach (var training in trainer.Trainings)
            {
                trainings.Add(MappTrainingEntityBaseToModelBase(training));
            }
            TrainerModel trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization, 
                Status = trainer.Status,
                Trainings = trainings

            };
            return trainerModel;
        }
     


        public TrainingModel MappTrainingEntityBaseToModelBase(TrainingEntityBase training)
        {
            TrainingModel trainingModel = new TrainingModel()
            {
                Id = training.Id,
                Name = training.Name,
                Description = training.Description
            };
            return trainingModel;
        }




        //Model - full info for Service layer 
        public GymModel MappGymEntityBaseToModel(GymWithTrainersAndTrainings gym)
        {
            var trainerModels = new List<TrainerModel>();
            foreach (var trainer in gym.Trainers)
            {
                trainerModels.Add(MappTrainerEntityBaseToModelBase(trainer));
            }

            var gymModel = new GymModel()
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Phone = gym.Phone,
                Trainers = trainerModels
            };
            return gymModel;
        }




        public TrainerModel MappTrainerWithGymAndTrainingsBaseToModel(TrainerWithGymAndTrainingsBase trainer)
        {
            GymModel gymModel = new GymModel();
            if (trainer.Gym != null)
            {
                gymModel = MappGymEntityBaseToModelBase(trainer.Gym);
            }           

            var trainings = new List<TrainingModel>();
            if (trainer.Trainings != null)
            {
                foreach (var training in trainer.Trainings)
                {
                    trainings.Add(MappTrainingEntityBaseToModelBase(training));
                }
            }
            

            var trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization,
                Status = trainer.Status,
                Gym = gymModel,
                Trainings = trainings
            };

            return trainerModel;
        }




        public TrainingModel MappGroupClassEntityBaseToModel(TrainingWithTrainerAndGymBase groupClass)
        {
            var trainerModels = new List<TrainerModel>();
            //foreach (var trainer in groupClass.Trainers)
            //{             
            //  trainerModels.Add(MappTrainerEntityToModelBase(trainer)); 
            //}

            var gymModels = new List<GymModel>();
            foreach (var gym in groupClass.Gyms)
            {
                gymModels.Add(MappGymEntityBaseToModelBase(gym));
            }

            TrainingModel groupClassModel = new TrainingModel()
            {
                Id = groupClass.Id,
                Name = groupClass.Name,
                Description = groupClass.Description,
                Trainers = trainerModels,
                Gyms = gymModels
            };

            return groupClassModel;
        }


        public SubscriptionModel MappSubscriptionPriceEntityBaseToModel(SubscriptionPriceBase subscriptionPrice)
        {
            SubscriptionModel subscriptionModel = new SubscriptionModel()
            {
                Id = subscriptionPrice.Id,
                GymId = subscriptionPrice.GymId,
                ValidDays = subscriptionPrice.ValidDays,
                GroupTraining = subscriptionPrice.GroupTraining,
                DietMonitoring = subscriptionPrice.DietMonitoring,
                Price = subscriptionPrice.Price
            };
            return subscriptionModel;
        }


        public PersonalTrainingEventModel MappEventEntityBaseToModel(PersonalTrainingEventEntityBase personalTrainingEventEntityBase)
        {
            PersonalTrainingEventModel personalTrainingEventModel = new PersonalTrainingEventModel()
            {
                Id = personalTrainingEventEntityBase.Id,
                Date = personalTrainingEventEntityBase.Date,
                StartTime = personalTrainingEventEntityBase.StartTime,
                EndTime = personalTrainingEventEntityBase.EndTime,
                TrainerId = personalTrainingEventEntityBase.TrainerId,               
                UserId = personalTrainingEventEntityBase.UserId,
                Status = personalTrainingEventEntityBase.Status
            };

            return personalTrainingEventModel;
        }

        public PersonalTrainingEventModel MappEventWithNamesBaseToModel(EventWithNamesBase eventEntityBase)
        {
            PersonalTrainingEventModel personalTrainingEventModel = new PersonalTrainingEventModel()
            {
                Id = eventEntityBase.Id,
                Date = eventEntityBase.Date,
                StartTime = eventEntityBase.StartTime,
                EndTime = eventEntityBase.EndTime,
                TrainerId = eventEntityBase.TrainerId,
                TrainerFirstName = eventEntityBase.TrainerFirstName,
                TrainerLastName = eventEntityBase.TrainerLastName,
                GymId = eventEntityBase.GymId,
                GymName = eventEntityBase.GymName,
                UserId = eventEntityBase.UserId,
                //UserName = personalTrainingEventEntityBase.UserName,
                UserFirstName = eventEntityBase.UserFirstName,
                UserLastName = eventEntityBase.UserLastName,
                Status = eventEntityBase.Status
            };

            return personalTrainingEventModel;
        }


        public GymWorkHoursModel MappGymWorkHoursEntityBaseToModel(GymWorkHoursEntityBase gymWorkHoursEntityBase)
        {
            GymWorkHoursModel gymWorkHoursModel = new GymWorkHoursModel()
            {
                Id = gymWorkHoursEntityBase.Id,
                DayName = gymWorkHoursEntityBase.DayOfWeekNumber,
                GymId = gymWorkHoursEntityBase.GymId,
                StartTime = gymWorkHoursEntityBase.StartTime,
                EndTime = gymWorkHoursEntityBase.EndTime
            };
            return gymWorkHoursModel;
        }


        public TrainerWorkHoursModel MappTrainerWorkHoursWithDaysBaseToModel(TrainerWorkHoursWithDayBase trainerWorkHoursWithDaysBase)
        {
            TrainerWorkHoursModel trainerWorkHoursModel = new TrainerWorkHoursModel()
            {
                Id = trainerWorkHoursWithDaysBase.Id,
                TrainerId = trainerWorkHoursWithDaysBase.TrainerId,
                StartTime = trainerWorkHoursWithDaysBase.StartTime,
                EndTime = trainerWorkHoursWithDaysBase.EndTime,
                GymWorkHoursId = trainerWorkHoursWithDaysBase.GymWorkHoursId,
                DayName = trainerWorkHoursWithDaysBase.DayName
            };
            return trainerWorkHoursModel;
        }
       


        public UserSubscriptionModel MappUserSubscriptionWithIncludedOptionsBaseToModel(UserSubscriptionWithIncludedOptionsBase subscriptionBase)
        {
            UserSubscriptionModel subscriptionModel = new UserSubscriptionModel()
            {
                Id = subscriptionBase.Id,
                UserId = subscriptionBase.UserId,
                GymSubscriptionId = subscriptionBase.GymSubscriptionId,
                TrainerId = subscriptionBase.TrainerId,
                StartDate = subscriptionBase.StartDate,
                EndDate = subscriptionBase.EndDate,
                GroupTraining = subscriptionBase.GroupTraining,
                DietMonitoring = subscriptionBase.DietMonitoring
            };
            return subscriptionModel;
        }

         //Chat
         public ChatMessageModel MapChatMessageEntityBaseToModel(ChatMessageEntityBase messageEntityBase)
         {
             ChatMessageModel messageModel = new ChatMessageModel()
             {
                 Id = messageEntityBase.Id,
                 ReceiverId = messageEntityBase.ReceiverId,
                 SenderId = messageEntityBase.SenderId,
                 Message = messageEntityBase.Message,
                 Date = messageEntityBase.Date
             };

             return messageModel;
         }







        //Model -> Entity
        public TrainerWithGymAndTrainingsBase MappTrainerModelToTrainerWithGymAndTrainingsBase(TrainerModel trainerModel)
        {
            List<TrainingEntityBase> trainingEntityBases = new List<TrainingEntityBase>();
            foreach (var training in trainerModel.Trainings)
            {
                trainingEntityBases.Add(new TrainingEntityBase()
                {
                    Id = training.Id                   
                });
            }
            
            TrainerWithGymAndTrainingsBase trainerBase = new TrainerWithGymAndTrainingsBase()
            {
                Id = trainerModel.Id,                
                Specialization = trainerModel.Specialization,
                Status = trainerModel.Status,
                Gym = new GymEntityBase()
                {
                    Id = trainerModel.Gym.Id                   
                },
                Trainings = trainingEntityBases
            };

            return trainerBase;
        }




        public TrainerEntityBase MappTrainerModelToEntityBase(TrainerModel trainerModel)
        {
            TrainerEntityBase trainerEntityBase = new TrainerEntityBase()
            {
                Id = trainerModel.Id,                
                Specialization = trainerModel.Specialization,
                GymId = trainerModel.Gym.Id,
                Status = trainerModel.Status
            };
            return trainerEntityBase;
        }


        


        public TrainerWorkHoursEntityBase MappTrainerWorkHoursModelToEntityBase(TrainerWorkHoursModel trainerWorkHoursModel)
        {
            TrainerWorkHoursEntityBase trainerWorkHourEntity = new TrainerWorkHoursEntityBase()
            {
                Id = trainerWorkHoursModel.Id,
                TrainerId = trainerWorkHoursModel.TrainerId,
                StartTime = trainerWorkHoursModel.StartTime,
                EndTime = trainerWorkHoursModel.EndTime,
                GymWorkHoursId = trainerWorkHoursModel.GymWorkHoursId
            };
            return trainerWorkHourEntity;
        }


        public ChatMessageEntityBase MapChatMessageModelToEntityBase(ChatMessageModel messageModel)
        {
            ChatMessageEntityBase messageEntityBase = new ChatMessageEntityBase()
            {
                ReceiverId = messageModel.ReceiverId,
                SenderId = messageModel.SenderId,
                Message = messageModel.Message,
                Date = messageModel.Date
            };

            return messageEntityBase;
        }

    }
}
