﻿using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FitMeApp.Services
{
    public sealed class ScheduleService: IScheduleService
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
                eventModels.Add(_mapper.MapEventEntityBaseToModel(entity));
            }
            return eventModels;
        }
        
        public int AddEvent(EventModel newEvent)
        {
            var eventEntityBase = _mapper.MapEventModelToEntityBase(newEvent);
            int eventId = _repository.AddEvent(eventEntityBase);
            return eventId;
        }
        
        public void DeleteEvent(int eventId)
        {
            _repository.DeleteEvent(eventId);
        }

        public IEnumerable<EventModel> GetEventsByUser(string userId)
        {
            var eventEntityBases = _repository.GetEventsByUser(userId);
            List<EventModel> eventModels = new List<EventModel>();
            foreach (var entity in eventEntityBases)
            {
                eventModels.Add(_mapper.MapEventEntityBaseToModel(entity));
            }
            return eventModels;
        }


        public IEnumerable<EventModel> GetEventsByUserAndDate(string userId, DateTime dateTime)
        {
            var eventWithNamesBases = _repository.GetEventsByUserAndDate(userId,dateTime);
            List<EventModel> eventModels = new List<EventModel>();
            foreach (var entity in eventWithNamesBases)
            {
                eventModels.Add(_mapper.MapEventWithNamesBaseToModel(entity));
            }
            return eventModels;
        }


        //public IEnumerable<EventModel> GetEventsByTrainerAndDate(string trainerId, DateTime dateTime)
        //{
        //    var eventWithNamesBases = _repository.GetEventsByTrainerAndDate(trainerId, dateTime);
        //    List<EventModel> eventModels = new List<EventModel>();
        //    foreach (var entity in eventWithNamesBases)
        //    {
        //        eventModels.Add(_mapper.MapEventWithNamesBaseToModel(entity));
        //    }
        //    return eventModels;
        //}

        public IEnumerable<EventModel> GetPersonalTrainingsByTrainerAndDate(string trainerId, DateTime dateTime)
        {
            var personalTrainingsWithNamesBases = _repository.GetPersonalTrainingsByTrainerAndDate(trainerId, dateTime);
            List<EventModel> eventModels = new List<EventModel>();
            foreach (var personalTraining in personalTrainingsWithNamesBases)
            {
                eventModels.Add(_mapper.MapEventWithNamesBaseToModel(personalTraining));
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


        public void ConfirmEvent(int eventId)
        {
            _repository.ConfirmEvent(eventId);
        }


        public int GetActualEventsCountByTrainer(string trainerId)
        {
            int actualEventsCount = _repository.GetActualEventsCountByTrainer(trainerId);
            return actualEventsCount;
        }

        public int GetOpenedEventsCountByTrainer(string trainerId)
        {
            int openedEventsCount = _repository.GetOpenedEventsByTrainer(trainerId).Count();
            return openedEventsCount;
        }

        public int GetEventsCount(string trainerId, List<DateTime> dates, int startTime, int endTime)
        {
            int eventsCount = _repository.GetEventsCount(trainerId, dates, startTime, endTime);
            return eventsCount;
        }

    }
}
