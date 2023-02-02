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

        public ScheduleService(IRepository repository, ILogger<ScheduleService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = new EntityModelMapper();

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

        public IEnumerable<EventModel> GetEventsByUser(string userId)
        {
            var eventEntityBases = _repository.GetEventsByUser(userId);
            List<EventModel> eventModels = new List<EventModel>();
            foreach (var entity in eventEntityBases)
            {
                eventModels.Add(_mapper.MappEventEntityBaseToModel(entity));
            }
            return eventModels;
        }

        public IEnumerable<EventModel> GetEventsByUserAndDate(string userId, DateTime dateTime)
        {
            var eventWithNamesBases = _repository.GetEventsByUserAndDate(userId,dateTime);
            List<EventModel> eventModels = new List<EventModel>();
            foreach (var entity in eventWithNamesBases)
            {
                eventModels.Add(_mapper.MappEventWithNamesBaseToModel(entity));
            }
            return eventModels;
        }


        public IEnumerable<EventModel> GetEventsByTrainerAndDate(string trainerId, DateTime dateTime)
        {
            var eventWithNamesBases = _repository.GetEventsByTrainerAndDate(trainerId, dateTime);
            List<EventModel> eventModels = new List<EventModel>();
            foreach (var entity in eventWithNamesBases)
            {
                eventModels.Add(_mapper.MappEventWithNamesBaseToModel(entity));
            }
            return eventModels;
        }

        public IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId)
        {
           var dateEventsCount = _repository.GetEventsCountForEachDateByUser(userId);
           return dateEventsCount;
        }

        public IDictionary<DateTime, int> GetEventsCountForEachDateByTrainer(string trainerId)
        {
            var dateEventsCount = _repository.GetEventsCountForEachDateByTrainer(trainerId);
            return dateEventsCount;
        }

        public bool ChangeEventStatus(int eventId)
        {
            bool result = _repository.ChangeEventStatus(eventId);
            return result;
        }
    }
}
