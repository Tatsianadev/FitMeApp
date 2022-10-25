
using FitMeApp.Contracts.Interfaces;
using FitMeApp.Services.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Mapping
{
    public class Mapper : IMapper
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;
        public Mapper(IRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger("MapperLogger");
        }
        public GymModel GetGymModel(int id)
        {
            var gymEntity = _repository.GetGym(id);
            var gymModel = new GymModel()
            {
                Id = gymEntity.Id,
                Name = gymEntity.Name,
                Address = gymEntity.Address,
                Phone = gymEntity.Phone,
                //TrainerStaff = _repository.
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
