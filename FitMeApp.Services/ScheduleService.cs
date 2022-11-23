using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace FitMeApp.Services
{
    public class ScheduleService: IScheduleService
    {
        private readonly IRepository _repository;
        private readonly ILogger _logger;
        private readonly EntityModelMapper _mapper;

        public ScheduleService(IRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger("ScheduleLogger");
            _mapper = new EntityModelMapper(loggerFactory);

        }



        public IEnumerable<EventModel> GetAllEvents() 
        {
            var eventEntityBases = _repository.GetAllEvents();
            List<EventModel> eventModels = new List<EventModel>();
            foreach (var entity in eventEntityBases)
            {
                eventModels.Add(_mapper.MappEventEntityBaseToModel(entity));
            }
            return eventModels;
        }
    }
}
