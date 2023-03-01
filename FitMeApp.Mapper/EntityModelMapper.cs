using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


namespace FitMeApp.Mapper
{
    public class EntityModelMapper
    {

        //ModelBase - Model with properties as EntityBase classes have. Without references to other classes (foreign key connections). 
        public GymModel MapGymEntityBaseToModelBase(GymEntityBase gym)
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

        public GymModel MapGymWithGalleryBaseToModelBase(GymWithGalleryBase gym)
        {
            GymModel gymModel = new GymModel()
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Phone = gym.Phone,
                GymImagePaths = gym.GymImagePaths
            };
            return gymModel;
        }

        public TrainerModel MapTrainerEntityBaseToModelBase(TrainerWithGymAndTrainingsBase trainer)
        {
            var trainings = new List<TrainingModel>();
            foreach (var training in trainer.Trainings)
            {
                trainings.Add(MapTrainingEntityBaseToModelBase(training));
            }
            TrainerModel trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                AvatarPath = trainer.AvatarPath,
                Specialization = trainer.Specialization, 
                Status = trainer.Status,
                Trainings = trainings

            };
            return trainerModel;
        }
     


        public TrainingModel MapTrainingEntityBaseToModelBase(TrainingEntityBase training)
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
        public GymModel MapGymEntityBaseToModel(GymWithTrainersAndTrainings gym)
        {
            var trainerModels = new List<TrainerModel>();
            foreach (var trainer in gym.Trainers)
            {
                trainerModels.Add(MapTrainerEntityBaseToModelBase(trainer));
            }

            var gymModel = new GymModel()
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Phone = gym.Phone,
                GymImagePaths = gym.GymImagePaths,
                Trainers = trainerModels
            };
            return gymModel;
        }




        public TrainerModel MapTrainerWithGymAndTrainingsBaseToModel(TrainerWithGymAndTrainingsBase trainer)
        {
            GymModel gymModel = new GymModel();
            if (trainer.Gym != null)
            {
                gymModel = MapGymEntityBaseToModelBase(trainer.Gym);
            }           

            var trainings = new List<TrainingModel>();
            if (trainer.Trainings != null)
            {
                foreach (var training in trainer.Trainings)
                {
                    trainings.Add(MapTrainingEntityBaseToModelBase(training));
                }
            }
            

            var trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                AvatarPath = trainer.AvatarPath,
                Specialization = trainer.Specialization,
                Status = trainer.Status,
                Gym = gymModel,
                Trainings = trainings
            };

            return trainerModel;
        }




        public TrainingModel MapGroupClassEntityBaseToModel(TrainingWithTrainerAndGymBase groupClass)
        {
            var trainerModels = new List<TrainerModel>();
            //foreach (var trainer in groupClass.Trainers)
            //{             
            //  trainerModels.Add(MappTrainerEntityToModelBase(trainer)); 
            //}

            var gymModels = new List<GymModel>();
            foreach (var gym in groupClass.Gyms)
            {
                gymModels.Add(MapGymEntityBaseToModelBase(gym));
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


        public SubscriptionModel MapSubscriptionPriceEntityBaseToModel(SubscriptionPriceBase subscriptionPrice)
        {
            SubscriptionModel subscriptionModel = new SubscriptionModel()
            {
                Id = subscriptionPrice.Id,
                GymId = subscriptionPrice.GymId,
                ValidDays = subscriptionPrice.ValidDays,
                GroupTraining = subscriptionPrice.GroupTraining,
                DietMonitoring = subscriptionPrice.DietMonitoring,
                WorkAsTrainer = subscriptionPrice.WorkAsTrainer,
                Price = subscriptionPrice.Price
            };
            return subscriptionModel;
        }


        public EventModel MapEventEntityBaseToModel(EventEntityBase eventEntityBase)
        {
            EventModel eventModel = new EventModel()
            {
                Id = eventEntityBase.Id,
                Date = eventEntityBase.Date,
                StartTime = eventEntityBase.StartTime,
                EndTime = eventEntityBase.EndTime,
                TrainerId = eventEntityBase.TrainerId,               
                UserId = eventEntityBase.UserId,              
                TrainingId = eventEntityBase.TrainingId,              
                Status = eventEntityBase.Status
            };

            return eventModel;
        }

        public EventModel MapEventWithNamesBaseToModel(EventWithNamesBase eventEntityBase)
        {
            EventModel eventModel = new EventModel()
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
                //UserName = eventEntityBase.UserName,
                UserFirstName = eventEntityBase.UserFirstName,
                UserLastName = eventEntityBase.UserLastName,
                TrainingId = eventEntityBase.TrainingId,
                TrainingName = eventEntityBase.TrainingName,
                Status = eventEntityBase.Status
            };

            return eventModel;
        }


        public GymWorkHoursModel MapGymWorkHoursEntityBaseToModel(GymWorkHoursEntityBase gymWorkHoursEntityBase)
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


        public TrainerWorkHoursModel MapTrainerWorkHoursWithDaysBaseToModel(TrainerWorkHoursWithDayBase trainerWorkHoursWithDaysBase)
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
       


        public UserSubscriptionModel MapUserSubscriptionFullInfoBaseToModel(UserSubscriptionFullInfoBase subscriptionBase)
        {
            UserSubscriptionModel subscriptionModel = new UserSubscriptionModel()
            {
                Id = subscriptionBase.Id,
                UserId = subscriptionBase.UserId,
                GymName = subscriptionBase.GymName,
                StartDate = subscriptionBase.StartDate,
                EndDate = subscriptionBase.EndDate,
                GroupTraining = subscriptionBase.GroupTraining,
                DietMonitoring = subscriptionBase.DietMonitoring,
                WorkAsTrainer = subscriptionBase.WorkAsTrainer,
                Price = subscriptionBase.Price
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


         public TrainerApplicationModel MapTrainerApplicationWithNamesBaseToModel(
             TrainerApplicationWithNamesBase trainerAppBase)
         {
             TrainerApplicationModel trainerAppModel = new TrainerApplicationModel()
             {
                 Id = trainerAppBase.Id,
                 UserId = trainerAppBase.UserId,
                 UserFirstName = trainerAppBase.UserFirstName,
                 UserLastName = trainerAppBase.UserLastName,
                 TrainerSubscription = trainerAppBase.TrainerSubscription,
                 ContractNumber = trainerAppBase.ContractNumber,
                 ApplicationDate = trainerAppBase.ApplicationDate
             };
             return trainerAppModel;
         }





        //Model -> Entity
        public TrainerWithGymAndTrainingsBase MapTrainerModelToTrainerWithGymAndTrainingsBase(TrainerModel trainerModel)
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




        public TrainerEntityBase MapTrainerModelToEntityBase(TrainerModel trainerModel)
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
        


        public TrainerWorkHoursEntityBase MapTrainerWorkHoursModelToEntityBase(TrainerWorkHoursModel trainerWorkHoursModel)
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



        public EventEntityBase MapEventModelToEntityBase(EventModel eventModel)
        {
            EventEntityBase eventEntityBase = new EventEntityBase()
            {
                Date = eventModel.Date,
                StartTime = eventModel.StartTime,
                EndTime = eventModel.EndTime,
                TrainerId = eventModel.TrainerId,
                UserId = eventModel.UserId,
                TrainingId = eventModel.TrainingId,
                Status = eventModel.Status
            };

            return eventEntityBase;
        }
    }
}
