using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;
using FitMeApp.Services.Contracts.Models.Chart;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

namespace FitMeApp.Services
{
    public sealed class GymService: IGymService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;
        private readonly ILogger _logger;
        public GymService(IRepository repository, ILogger<GymService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = new EntityModelMapper();
        }
        

        
        public IEnumerable<GymModel> GetAllGymModels()
        {
            try
            {
                var gymEntityBases = _repository.GetAllGyms();
                var gymsModels = new List<GymModel>();

                foreach (var gym in gymEntityBases)
                {                    
                    gymsModels.Add(_mapper.MapGymEntityBaseToModelBase(gym));
                }
                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public IEnumerable<GymModel> GetAllGymsWithGalleryModels()
        {
            try
            {
                var gymEntityBases = _repository.GetAllGymsWithGallery();
                var gymsModels = new List<GymModel>();

                foreach (var gym in gymEntityBases)
                {
                    gymsModels.Add(_mapper.MapGymWithGalleryBaseToModelBase(gym));
                }
                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public GymModel GetGymModel(int id)
        {
            try
            {
                var gymEntityBase = _repository.GetGymWithTrainersAndTrainings(id);
                GymModel gym = _mapper.MapGymEntityBaseToModel(gymEntityBase);
                return gym;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public IEnumerable<GymModel> GetGymsByTrainings(List<int> trainingsId)
        {
            try
            {
                var gymsByTrainings = _repository.GetGymsByTrainings(trainingsId);
                List<GymModel> gyms = new List<GymModel>();
                foreach (var gym in gymsByTrainings)
                {
                    gyms.Add(_mapper.MapGymWithGalleryBaseToModelBase(gym));
                }
                return gyms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
           
        }


        public IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId)
        {
            try
            {
                var workHoursEntityBase = _repository.GetWorkHoursByGym(gymId);
                List<GymWorkHoursModel> workHoursModels = new List<GymWorkHoursModel>();
                foreach (var item in workHoursEntityBase)
                {
                    workHoursModels.Add(_mapper.MapGymWorkHoursEntityBaseToModel(item));
                }
                return workHoursModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek)
        {
            try
            {
                int gymWorkHoursId = _repository.GetGymWorkHoursId(gymId, dayOfWeek);
                return gymWorkHoursId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            
        }


        public int GetGymIdByTrainer(string trainerId)
        {
            try
            {
                int gymId = _repository.GetGymIdByTrainer(trainerId);
                return gymId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        


        //Charts

        public AttendanceChartModel GetAttendanceChartDataForCertainDayByGym(int gymId, DayOfWeek day)
        {
            try
            {
                var attendanceChartEntities = _repository.GetNumOfVisitorsPerHourOnCertainDayByGym(gymId, day).ToList();
                if (attendanceChartEntities.Any())
                {
                    var attendanceCharModel = _mapper.MapNumberOfVisitorsPerHourEntityBaseToAttendanceModel(attendanceChartEntities);
                    return attendanceCharModel;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            
        }


        public void AddVisitingChartDataToDb(IEnumerable<AttendanceChartModel> data)
        {
            var entities = new List<NumberOfVisitorsPerHourEntityBase>();
            try
            {
                foreach (var dayData in data)
                {
                    entities.AddRange(_mapper.MapVisitingChartModelToNumberOfVisitorsPerHourEntityBase(dayData));
                }

                _repository.DeleteNumberOfVisitorsPerHourChartData(entities.Select(x => x.GymId).First());
                _repository.AddNumberOfVisitorsPerHourChartData(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
           
           
        }

    }
}
