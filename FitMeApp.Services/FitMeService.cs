using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FitMeApp.Services
{
    public class FitMeService: IFitMeService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;
        private readonly ILogger _logger;
        public FitMeService(IRepository repository, ILogger<FitMeService> logger)
        {
            _repository = repository;
            _logger = logger;
            _mapper = new EntityModelMapper();
        }

        //Gym
        
        public IEnumerable<GymModel> GetAllGymModels()
        {
            try
            {
                var gymEntityBases = _repository.GetAllGyms();
                var gymsModels = new List<GymModel>();

                foreach (var gym in gymEntityBases)
                {                    
                    gymsModels.Add(_mapper.MappGymEntityBaseToModelBase(gym));
                }
                return gymsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

        }

        public GymModel GetGymModel(int id)
        {
            var gymEntityBase = _repository.GetGymWithTrainersAndTrainings(id);           
            GymModel gym = _mapper.MappGymEntityBaseToModel(gymEntityBase);
            return gym;
        }


        public IEnumerable<GymModel> GetGymsByTrainings(List<int> trainingsId)
        {
            var gymsByTrainings = _repository.GetGymsByTrainings(trainingsId);
            List<GymModel> gyms = new List<GymModel>();
            foreach (var gym in gymsByTrainings)
            {
                gyms.Add(_mapper.MappGymEntityBaseToModelBase(gym));
            }
            return gyms;
        }

        public IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId)
        {
            var workHoursEntityBase = _repository.GetWorkHoursByGym(gymId);
            List<GymWorkHoursModel> workHoursModels = new List<GymWorkHoursModel>();
            foreach (var item in workHoursEntityBase)
            {
                workHoursModels.Add(_mapper.MappGymWorkHoursEntityBaseToModel(item));
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
            int gymId = _repository.GetTrainer(trainerId).GymId;
            return gymId;
        }



        //Training

        public ICollection<TrainingModel> GetAllTrainingModels()
        {
            try
            {
                var trainings = _repository.GetAllTrainings();
                var trainingModels = new List<TrainingModel>();

                foreach (var groupClass in trainings)
                {                    
                    trainingModels.Add(_mapper.MappTrainingEntityBaseToModelBase(groupClass));
                }
                return trainingModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public TrainingModel GetTrainingModel(int trainingId)
        {
            var trainingEntity = _repository.GetTraining(trainingId);
            TrainingModel trainingModel = _mapper.MappTrainingEntityBaseToModelBase(trainingEntity);
            return trainingModel;
        }



      


        //Subscriptions

        public IEnumerable<SubscriptionModel> GetSubscriptionsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetSubscriptionsByGymByFilter(gymId, periods, groupTraining, dietMonitoring);
                foreach (var subscription in subscriptionsEntity)
                {
                    subscriptionsModels.Add(_mapper.MappSubscriptionPriceEntityBaseToModel(subscription));
                }
                return subscriptionsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }

        }


        public IEnumerable<SubscriptionModel> GetAllSubscriptionsByGym(int gymId)
        {
            try
            {
                List<SubscriptionModel> subscriptionsModels = new List<SubscriptionModel>();
                var subscriptionsEntity = _repository.GetAllSubscriptionsByGym(gymId);
                foreach (var subscription in subscriptionsEntity)
                {
                    subscriptionsModels.Add(_mapper.MappSubscriptionPriceEntityBaseToModel(subscription));
                }
                return subscriptionsModels;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }           
        }

        public List<int> GetAllSubscriptionPeriods()
        {
            List<int> allSubscriptionPeriods = _repository.GetAllSubscriptionPeriods();
            return allSubscriptionPeriods;
        }

        public SubscriptionModel GetSubscriptionByGym(int subscriptionId, int gymId)
        {
            var subscriptionPriceEntityBase = _repository.GetSubscriptionWithPriceByGym(subscriptionId, gymId);
            SubscriptionModel subscriptionModel = _mapper.MappSubscriptionPriceEntityBaseToModel(subscriptionPriceEntityBase);
            return subscriptionModel;
        }

        //UserSubscriptions
        public bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate)
        {
            bool result = _repository.AddUserSubscription(userId, gymId, subscriptionId, startDate);
            return result;
        }

        public int GetActualSubscriptionsCountByTrainer(string trainerId)
        {
            int actualSubscriptionsCount = _repository.GetActualSubscriptionsCountByTrainer(trainerId);
            return actualSubscriptionsCount;
        }

        public IEnumerable<UserSubscriptionModel> GetUserSubscriptions(string userId)
        {
            var userSubscrioptionsBase = _repository.GetUserSubscriptionsFullInfo(userId);
            List<UserSubscriptionModel> userSubscriptionsModel = new List<UserSubscriptionModel>();
            foreach (var baseItem in userSubscrioptionsBase)
            {
                userSubscriptionsModel.Add(_mapper.MappUserSubscriptionWithIncludedOptionsBaseToModel(baseItem));
            }           
            return userSubscriptionsModel;
        }

        //Trainers

        public List<TrainerModel> GetAllTrainerModels()
        {
            var trainersEntity = _repository.GetAllTrainersWithGymAndTrainings();
            List<TrainerModel> trainers = new List<TrainerModel>();
            foreach (var trainerEntity in trainersEntity)
            {
                trainers.Add(_mapper.MappTrainerWithGymAndTrainingsBaseToModel(trainerEntity));
            }
            return trainers;
        }

        public IEnumerable<TrainerModel> GetAllTrainersNames()
        {
            var trainersEntity = _repository.GetAllTrainersWithNames();
            List<TrainerModel> trainers = new List<TrainerModel>();
            foreach (var trainerEntity in trainersEntity)
            {
                trainers.Add(_mapper.MappTrainerWithGymAndTrainingsBaseToModel(trainerEntity));
            }
            return trainers;
        }

        public TrainerModel GetTrainerWithGymAndTrainings(string trainerId)
        {
            var trainerWithGymAndTrainings = _repository.GetTrainerWithGymAndTrainings(trainerId);
            TrainerModel trainer = _mapper.MappTrainerWithGymAndTrainingsBaseToModel(trainerWithGymAndTrainings);
            return trainer;
        }


        public IEnumerable<TrainerWorkHoursModel> GetWorkHoursByTrainer(string trainerId)
        {
            var workHoursEntityBase = _repository.GetWorkHoursByTrainer(trainerId);
            List<TrainerWorkHoursModel> workHoursModels = new List<TrainerWorkHoursModel>();
            foreach (var item in workHoursEntityBase)
            {
                workHoursModels.Add(_mapper.MappTrainerWorkHoursWithDaysBaseToModel(item));
            }
            return workHoursModels;
        }


        public bool UpdateTrainerWithGymAndTrainings(TrainerModel newTrainerInfo)
        {
            var trainerBase = _mapper.MappTrainerModelToTrainerWithGymAndTrainingsBase(newTrainerInfo);
            bool result = _repository.UpdateTrainerWithGymAndTrainings(trainerBase);
            return result;
        }


        public void UpdateTrainerStatus(string trainerId, TrainerApproveStatusEnum newStatus)
        {
            var trainer = _repository.GetTrainer(trainerId);
            trainer.Status = newStatus;
           _repository.UpdateTrainer(trainer);            
        }

        public bool CheckFacilityUpdateTrainerWorkHoursByGymScedule(int gymId, List<TrainerWorkHoursModel> newWorkHours)
        {
            List<DayOfWeek> gymWorkDayes = _repository.GetWorkHoursByGym(gymId).Select(x => x.DayOfWeekNumber).ToList();
            List<DayOfWeek> newTrainerWorkDayes = newWorkHours.Select(x => x.DayName).ToList();
            if (newTrainerWorkDayes.Except(gymWorkDayes).Count() > 0) // проверка, тренер работает только в дни, когда работает зал
            {
                return false;
            }

            var gymWorkHours =_repository.GetWorkHoursByGym(gymId);          
            foreach (var newTrainerWorkHours in newWorkHours) //проверка, чтобы рабочие часы тренера не выходили за пределы рабочих часов зала
            {
                var gymWorkStartTime = gymWorkHours.Where(x => x.Id == newTrainerWorkHours.GymWorkHoursId).First().StartTime;
                var gymWorkEndTime = gymWorkHours.Where(x => x.Id == newTrainerWorkHours.GymWorkHoursId).First().EndTime;
                if (gymWorkStartTime > newTrainerWorkHours.StartTime || gymWorkEndTime < newTrainerWorkHours.EndTime)
                {
                    return false;
                }
            }

            return true;

        }


        public bool CheckFacilityUpdateTrainerWorkHoursByEvents(List<TrainerWorkHoursModel> newWorkHours)
        {
            string trainerId = newWorkHours.Select(x => x.TrainerId).First().ToString();
            var actualEvents = _repository.GetActualEventsByTrainer(trainerId); 
            if (actualEvents.Count() == 0)
            {
                return true;
            }

            var allNeededDayesOfWeek = actualEvents.Select(x => x.Date.DayOfWeek).Distinct();
            var newWorkDayesOfWeek = newWorkHours.Select(x => x.DayName).Distinct();
            if (allNeededDayesOfWeek.Except(newWorkDayesOfWeek).Count() > 0)
            {
                return false;
            }

            foreach (var eventItem in actualEvents)
            {
                foreach (var newTrainerWorkHours in newWorkHours)
                {
                    if (eventItem.Date.DayOfWeek == newTrainerWorkHours.DayName)
                    {
                        if (eventItem.StartTime <= newTrainerWorkHours.StartTime || eventItem.EndTime >= newTrainerWorkHours.EndTime)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;

        }

        public bool CheckFacilityUpdateTrainerWorkHours(List<TrainerWorkHoursModel> newWorkHours)
        {
            string trainerId = newWorkHours.Select(x => x.TrainerId).First().ToString();
            int gymId = _repository.GetTrainer(trainerId).GymId;            
            if (CheckFacilityUpdateTrainerWorkHoursByEvents(newWorkHours) && CheckFacilityUpdateTrainerWorkHoursByGymScedule(gymId,newWorkHours))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateTrainerWorkHours(List<TrainerWorkHoursModel> trainerWorkHours)
        {
            string trainerId = trainerWorkHours.Select(x => x.TrainerId).First();            
            List<int> previousTrainerWorkHoursId = _repository.GerAllTrainerWorkHoursId(trainerId).ToList();
            List<int> newTrainerWorkHoursId = trainerWorkHours.Select(x => x.Id).ToList();

            bool result = true;
           
            List<int> idRowsToDelete = previousTrainerWorkHoursId.Except(newTrainerWorkHoursId).ToList();
            foreach (var workHoursId in idRowsToDelete)
            {
                result = _repository.DeleteTrainerWorkHours(workHoursId);
                if (result == false)
                {
                    return false;
                }               
            }

           
            List<TrainerWorkHoursModel> rowsToAdd = trainerWorkHours.Where(x => x.Id == 0).ToList();
            var entityRowsToAdd = rowsToAdd.Select(model => _mapper.MappTrainerWorkHoursModelToEntityBase(model)).ToList();
            foreach (var workHoursToAdd in entityRowsToAdd)
            {                
                result = _repository.AddTrainerWorkHours(workHoursToAdd);
                if (result == false)
                {
                    return false;
                }
            }

            List<TrainerWorkHoursModel> rowsToUpdate = trainerWorkHours.Where(x => x.Id != 0).ToList();
            var entityRowsToUpdate = rowsToUpdate.Select(model => _mapper.MappTrainerWorkHoursModelToEntityBase(model)).ToList();
            foreach (var workHoursToUpdate in entityRowsToUpdate)
            {
                result = _repository.UpdateTrainerWorkHours(workHoursToUpdate);
                if (result == false)
                {
                    return false;
                }
            }

            return true;

        }


        public IEnumerable<string> GetAllClientsIdByTrainer(string trainerId)
        {
            List<string> clientsId = _repository.GetAllClientsIdByTrainer(trainerId).ToList();
            return clientsId;
        }


        public IEnumerable<TrainerModel> GetTrainersByFilter(List<GenderEnum> selectedGenders, List<TrainerSpecializationsEnum> selectedSpecializations)
        {
            List<string> selectedGendersString = new List<string>();
            foreach (var item in selectedGenders)
            {
                selectedGendersString.Add(item.ToString());
            }

            List<string> selectedSpecializationsString = new List<string>();
            foreach (var item in selectedSpecializations)
            {
                selectedSpecializationsString.Add(item.ToString());
            }

            var trainersBaseByFilter = _repository.GetTrainersWithGymAndTrainengsByFilter(selectedGendersString, selectedSpecializationsString);
            List<TrainerModel> trainerModels = new List<TrainerModel>();
            foreach (var trainerBase in trainersBaseByFilter)
            {
                trainerModels.Add(_mapper.MappTrainerWithGymAndTrainingsBaseToModel(trainerBase));
            }
            return trainerModels;
        }


        public bool DeleteTrainer(string id)
        {
            bool result = _repository.DeleteTrainer(id);
            return result;
        }


        public bool DeleteTrainerWorkHoursByTrainer(string trainerId)
        {
            bool result = _repository.DeleteTrainerWorkHoursByTrainer(trainerId);
            return result;
        }

        public bool AddTrainer(TrainerModel trainer)
        {
            var trainerEntityBase = _mapper.MappTrainerModelToEntityBase(trainer);
            bool result = _repository.AddTrainer(trainerEntityBase);
            return result;            
        }


        //Events
        public int GetActualEventsCountByTrainer(string trainerId)
        {
            int actualEventsCount = _repository.GetActualEventsCountByTrainer(trainerId);
            return actualEventsCount;
        }

        //TrainingTrainer
        public bool DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId)
        {
            bool result = _repository.DeleteAllTrainingTrainerConnectionsByTrainer(trainerId);
            return result;
        }

        public bool AddTrainingTrainerConnection(string trainerId, int trainingId)
        {
            bool result = _repository.AddTrainingTrainerConnection(trainerId, trainingId);
            return result;
        }

    }
}
