using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;


namespace FitMeApp.Mapper
{
    public class EntityModelMapper
    {

        private readonly ILogger _logger;
        public EntityModelMapper(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger("MapperLogger");
        }

        //ModelBase - Model with properties as EntityBase classes have. Without references to other classes (foreign key connections). 
        private GymModel MappGymEntityToModelBase(GymEntityBase gym)
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

        private TrainerModel MappTrainerEntityToModelBase(TrainerWithGymAndTrainingsBase trainer)
        {
            var trainings = new List<GroupClassModel>();
            foreach (var training in trainer.Trainings)
            {
                trainings.Add(MappGroupClassEntityToModelBase(training));
            }
            TrainerModel trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization,
                GroupClasses = trainings
                
            };
            return trainerModel;
        }

        private GroupClassModel MappGroupClassEntityToModelBase(TrainingEntityBase groupClass)
        {
            GroupClassModel groupClassModel = new GroupClassModel()
            {
                Id = groupClass.Id,
                Name = groupClass.Name,
                Description = groupClass.Description
            };
            return groupClassModel;
        }



        public GymModel MappGymEntityBaseToModel(GymWithTrainersAndTrainings gym)
        {
            var trainerModels = new List<TrainerModel>();
            foreach (var trainer in gym.Trainers)
            {
                trainerModels.Add(MappTrainerEntityToModelBase(trainer));
            }

            //var groupClassModels = new List<GroupClassModel>();
            //foreach (var groupClass in gym.GroupClasses)
            //{
            //    groupClassModels.Add(MappGroupClassEntityToModelBase(groupClass));
            //}

            var gymModel = new GymModel()
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Phone = gym.Phone,
                TrainerStaff = trainerModels                
            };
            return gymModel;
        }




        public TrainerModel MappTrainerEntityBaseToModel(TrainerWithGymAndTrainingsBase trainer)
        {
            var gymModel = new GymModel();
            gymModel = MappGymEntityToModelBase(trainer.Gym);

            var groupClassModels = new List<GroupClassModel>();
            foreach (var groupClass in trainer.Trainings)
            {
                groupClassModels.Add(MappGroupClassEntityToModelBase(groupClass));
            }

            var trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization,
                Gym = gymModel,
                GroupClasses = groupClassModels
            };

            return trainerModel;
        }



        public GroupClassModel MappGroupClassEntityBaseToModel(TrainingWithTrainerAndGymBase groupClass)
        {
            var trainerModels = new List<TrainerModel>(); 
            //foreach (var trainer in groupClass.Trainers)
            //{             
            //  trainerModels.Add(MappTrainerEntityToModelBase(trainer)); 
            //}

            var gymModels = new List<GymModel>();
            foreach (var gym in groupClass.Gyms)
            {                
               gymModels.Add(MappGymEntityToModelBase(gym));                
            }

            GroupClassModel groupClassModel = new GroupClassModel()
            {
                Id = groupClass.Id,
                Name = groupClass.Name,
                Description = groupClass.Description,
                Trainers = trainerModels,
                Gyms = gymModels
            };

            return groupClassModel;
        }
    }
}
