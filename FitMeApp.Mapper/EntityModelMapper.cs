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
        private GymModel ConvertToGymModelBase(GymEntityBase gym)
        {
            GymModel gymModel =  new GymModel()
            {
                Id = gym.Id,
                Name = gym.Name,
                Address = gym.Address,
                Phone = gym.Phone
            };
            return gymModel;
        }

        private TrainerModel ConvertToTrainerModelBase(TrainerEntityBase trainer)
        {
            TrainerModel trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization
            };
            return trainerModel;
        }

        private GroupClassModel ConvertToGroupClassModelBase(GroupClassEntityBase groupClass)
        {
            GroupClassModel groupClassModel = new GroupClassModel()
            {
                Id = groupClass.Id,
                Name = groupClass.Name,
                Description = groupClass.Description
            };
            return groupClassModel;
        }



        public GymModel ConvertToGymModel(GymEntityBase entityBase, IEnumerable<TrainerEntityBase> trainers, IEnumerable<GroupClassEntityBase> groupClasses)
        {
            var trainerModels = new List<TrainerModel>();
            var groupClassModels = new List<GroupClassModel>();

            foreach (var trainer in trainers)
            {
                trainerModels.Add(ConvertToTrainerModelBase(trainer));
            }

            foreach (var groupClass in groupClasses)
            {
                groupClassModels.Add(ConvertToGroupClassModelBase(groupClass));
            }

            var gymModel = new GymModel()
            {
                Id = entityBase.Id,
                Name = entityBase.Name,
                Address = entityBase.Address,
                Phone = entityBase.Phone,
                TrainerStaff = trainerModels,
                GroupClasses =groupClassModels
            };

            return gymModel;
        }

       
      

     

        public TrainerModel ConvertToTrainerModel(TrainerEntityBase trainer, IEnumerable<GymEntityBase> gyms, IEnumerable<GroupClassEntityBase> groupClasses)
        {
            var gymModels = new List<GymModel>();
            var groupClassModels = new List<GroupClassModel>();
           
            foreach (var gym in gyms)
            {
                gymModels.Add(ConvertToGymModelBase(gym));
            }

            foreach (var groupClass in groupClasses)
            {
                groupClassModels.Add(ConvertToGroupClassModelBase(groupClass));
            }

            var trainerModel = new TrainerModel()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization,
                Gyms = gymModels,
                GroupClasses = groupClassModels
            };

            return trainerModel;
        }

        public GroupClassModel GetGroupClassModel(GroupClassEntityBase groupClass, IEnumerable<TrainerEntityBase> trainers, IEnumerable<GymEntityBase> gyms)
        {
            var trainerModels = new List<TrainerModel>();
            var gymModels = new List<GymModel>();
            int trainerId = 0;
            int gymId = 0;
            foreach (var trainer in trainers)
            {                
                if (trainerId != trainer.Id)
                {
                    trainerModels.Add(ConvertToTrainerModelBase(trainer));
                }                
                trainerId = trainer.Id;
            }

            foreach (var gym in gyms)
            {
                if (gymId != gym.Id)
                {
                    gymModels.Add(ConvertToGymModelBase(gym));
                }
                gymId = gym.Id;
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
