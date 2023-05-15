using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;
using FitMeApp.Services.Contracts.Models.Chart;

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
            var gymEntityBase = _repository.GetGymWithTrainersAndTrainings(id);           
            GymModel gym = _mapper.MapGymEntityBaseToModel(gymEntityBase);
            return gym;
        }


        public IEnumerable<GymModel> GetGymsByTrainings(List<int> trainingsId)
        {
            var gymsByTrainings = _repository.GetGymsByTrainings(trainingsId);
            List<GymModel> gyms = new List<GymModel>();
            foreach (var gym in gymsByTrainings)
            {
                gyms.Add(_mapper.MapGymWithGalleryBaseToModelBase(gym));
            }
            return gyms;
        }


        public IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId)
        {
            var workHoursEntityBase = _repository.GetWorkHoursByGym(gymId);
            List<GymWorkHoursModel> workHoursModels = new List<GymWorkHoursModel>();
            foreach (var item in workHoursEntityBase)
            {
                workHoursModels.Add(_mapper.MapGymWorkHoursEntityBaseToModel(item));
            }
            return workHoursModels;
        }


        public int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek)
        {
            int gymWorkHoursId = _repository.GetGymWorkHoursId(gymId, dayOfWeek);
            return gymWorkHoursId;
        }


        public int GetGymIdByTrainer(string trainerId)
        {
            int gymId = _repository.GetGymIdByTrainer(trainerId);
            return gymId;
        }
        


        //Charts

        public AttendanceChartModel GetAttendanceChartDataForCertainDayByGym(int gymId, DayOfWeek day)
        {
            var attendanceChartEntities = _repository.GetNumOfVisitorsPerHourOnCertainDayByGym(gymId, day);
            if (attendanceChartEntities.Count() != 0)
            {
                var attendanceCharModel = _mapper.MapNumberOfVisitorsPerHourEntityBaseToAttendanceModel(attendanceChartEntities);
                return attendanceCharModel;
            }
            
            return null;
        }

    }
}
