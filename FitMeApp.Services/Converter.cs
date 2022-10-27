
using FitMeApp.Contracts.BaseEntities;
using FitMeApp.Contracts.Interfaces;
using FitMeApp.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services
{
    public class Converter 
    {
        
        private readonly ILogger _logger;
        public Converter(ILoggerFactory loggerFactory)
        {           
            _logger = loggerFactory.CreateLogger("MapperLogger");
        }


        public GymModel ConvertToGymModel(GymEntityBase entityBase, IEnumerable<TrainerEntityBase> trainers, IEnumerable<GroupClassEntityBase> groupClasses)
        {
            var trainerModels = new List<TrainerModel>();
            var groupClassModels = new List<GroupClassModel>();

            foreach (var trainer in trainers)
            {
                trainerModels.Add(new TrainerModel()
                {
                    Id = trainer.Id,
                    FirstName = trainer.FirstName,
                    LastName = trainer.LastName,
                    Gender = trainer.Gender,
                    Picture = trainer.Picture,
                    Specialization=trainer.Specialization
                });
            }

            foreach (var groupClass in groupClasses)
            {
                groupClassModels.Add(new GroupClassModel()
                {
                    Id = groupClass.Id,
                    Name = groupClass.Name,
                    Description = groupClass.Description
                });
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

       
        public GroupClassModel GetGroupClassModel(int id)
        {
            throw new NotImplementedException();
        }

     

        public TrainerModel ConvertToTrainerModel(TrainerEntityBase trainer, IEnumerable<GymEntityBase> gyms, IEnumerable<GroupClassEntityBase> groupClasses)
        {
            var gymModels = new List<GymModel>();
            var groupClassModels = new List<GroupClassModel>();
           
            foreach (var gym in gyms)
            {
                gymModels.Add(new GymModel()
                {
                    Id = gym.Id,
                    Name = gym.Name,
                    Address = gym.Address,
                    Phone = gym.Phone
                });
            }

            foreach (var groupClass in groupClasses)
            {
                groupClassModels.Add(new GroupClassModel()
                {
                    Id = groupClass.Id,
                    Name = groupClass.Name,
                    Description = groupClass.Description
                });
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
    }
}
