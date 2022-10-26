
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


        public GymModel ConvertToGymModel(GymEntityBase entityBase, IEnumerable<TrainerEntityBase> trainers)
        {
            var trainerModels = new List<TrainerModel>(); 
            foreach (var trainer in trainers)
            {
                trainerModels.Add(new TrainerModel()
                {
                    Id = trainer.Id,
                    FirstName = trainer.FirstName,
                    LastName = trainer.LastName,
                    Gender = trainer.Gender,
                    Picture = trainer.Picture
                });
            }

            var gymModel = new GymModel()
            {
                Id = entityBase.Id,
                Name = entityBase.Name,
                Address = entityBase.Address,
                Phone = entityBase.Phone,
                TrainerStaff = trainerModels
            };

            return gymModel;
        }

       
        public GroupClassModel GetGroupClassModel(int id)
        {
            throw new NotImplementedException();
        }

     

        public TrainerModel GetTrainerModel(int id)
        {
            throw new NotImplementedException();
        }
    }
}
