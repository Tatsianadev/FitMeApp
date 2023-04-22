using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Repository.EntityFramework.Entities;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services
{
    public sealed class TrainerService : ITrainerService
    {
        private readonly IRepository _repository;
        private readonly EntityModelMapper _mapper;

        public TrainerService(IRepository repository)
        {
            _repository = repository;
            _mapper = new EntityModelMapper();
        }


        public List<TrainerModel> GetAllTrainerModels()
        {
            var trainersEntity = _repository.GetAllTrainersWithGymAndTrainings();
            List<TrainerModel> trainers = new List<TrainerModel>();
            foreach (var trainerEntity in trainersEntity)
            {
                trainers.Add(_mapper.MapTrainerWithGymAndTrainingsBaseToModel(trainerEntity));
            }
            return trainers;
        }


        public TrainerModel GetTrainerWithGymAndTrainings(string trainerId)
        {
            var trainerWithGymAndTrainings = _repository.GetTrainerWithGymAndTrainings(trainerId);
            TrainerModel trainer = _mapper.MapTrainerWithGymAndTrainingsBaseToModel(trainerWithGymAndTrainings);
            return trainer;
        }


        public int GetGymIdByTrainer(string trainerId)
        {
            int gymId = _repository.GetGymIdByTrainer(trainerId);
            return gymId;
        }


        public IEnumerable<TrainerWorkHoursModel> GetWorkHoursByTrainer(string trainerId)
        {
            var workHoursEntityBase = _repository.GetWorkHoursByTrainer(trainerId);
            List<TrainerWorkHoursModel> workHoursModels = new List<TrainerWorkHoursModel>();
            foreach (var item in workHoursEntityBase)
            {
                workHoursModels.Add(_mapper.MapTrainerWorkHoursWithDaysBaseToModel(item));
            }
            return workHoursModels;
        }


        public void UpdateTrainerWithGymAndTrainings(TrainerModel newTrainerInfo)
        {
            TrainerEntityBase newTrainerInfoBase = new TrainerEntityBase()
            {
                Id = newTrainerInfo.Id,
                Specialization = newTrainerInfo.Specialization
            };
            _repository.UpdateTrainer(newTrainerInfoBase);

            var previousTrainingsId = _repository.GetAllTrainingIdsByTrainer(newTrainerInfo.Id);
            var newTrainingsId = newTrainerInfo.Trainings.Select(x => x.Id).ToList();

            var trainingsIdToDelete = previousTrainingsId.Except(newTrainingsId);
            var trainingsIdToAdd = newTrainingsId.Except(previousTrainingsId);

            foreach (var trainingId in trainingsIdToDelete)
            {
                _repository.DeleteTrainingTrainerConnection(newTrainerInfo.Id, trainingId);
            }

            foreach (var trainingId in trainingsIdToAdd)
            {
                _repository.AddTrainingTrainerConnection(newTrainerInfo.Id, trainingId);
            }
        }


        private bool CheckFacilityUpdateTrainerWorkHoursByGymSchedule(int gymId, List<TrainerWorkHoursModel> newWorkHours)
        {
            var gymWorkHours = _repository.GetWorkHoursByGym(gymId);

            List<DayOfWeek> gymWorkDayes = gymWorkHours.Select(x => x.DayOfWeekNumber).ToList();
            List<DayOfWeek> newTrainerWorkDayes = newWorkHours.Select(x => x.DayName).ToList();
            if (newTrainerWorkDayes.Except(gymWorkDayes).Count() > 0) //check: Trainer works only the DAYS, when the Gym open 
            {
                return false;
            }

            foreach (var newTrainerWorkHours in newWorkHours) // check: Trainer works only the HOURS, when the Gym open
            {
                var gymWorkHoursById = gymWorkHours.Where(x => x.Id == newTrainerWorkHours.GymWorkHoursId).FirstOrDefault();
                if (gymWorkHoursById != null)
                {
                    var gymWorkStartTime = gymWorkHoursById.StartTime;
                    var gymWorkEndTime = gymWorkHoursById.EndTime;
                    if (gymWorkStartTime > newTrainerWorkHours.StartTime || gymWorkEndTime < newTrainerWorkHours.EndTime)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }


        private bool CheckFacilityUpdateTrainerWorkHoursByEvents(List<TrainerWorkHoursModel> newWorkHours)
        {
            string trainerId = newWorkHours.First().TrainerId;
            var actualEvents = _repository.GetActualEventsByTrainer(trainerId);
            if (actualEvents.Count() == 0)
            {
                return true;
            }

            var allNeededDaysOfWeek = actualEvents.Select(x => x.Date.DayOfWeek).Distinct();
            var newWorkDaysOfWeek = newWorkHours.Select(x => x.DayName).Distinct();
            if (allNeededDaysOfWeek.Except(newWorkDaysOfWeek).Any())
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
            int licenseId = _repository.GetTrainer(trainerId).WorkLicenseId;
            int gymId = _repository.GetTrainerWorkLicense(licenseId).GymId;
            if (CheckFacilityUpdateTrainerWorkHoursByEvents(newWorkHours) && CheckFacilityUpdateTrainerWorkHoursByGymSchedule(gymId, newWorkHours))
            {
                return true;
            }

            return false;
        }


        public bool UpdateTrainerWorkHours(List<TrainerWorkHoursModel> trainerWorkHours)
        {
            string trainerId = trainerWorkHours.Select(x => x.TrainerId).First();
            List<int> previousTrainerWorkHoursId = _repository.GerAllTrainerWorkHoursId(trainerId).ToList();
            List<int> newTrainerWorkHoursId = trainerWorkHours.Select(x => x.Id).ToList();

            List<int> workHoursIdsToDelete = previousTrainerWorkHoursId.Except(newTrainerWorkHoursId).ToList();
            foreach (var workHoursId in workHoursIdsToDelete)
            {
                _repository.DeleteTrainerWorkHours(workHoursId);
            }

            List<TrainerWorkHoursModel> workHoursToAdd = trainerWorkHours.Where(x => x.Id == 0).ToList();
            var workHoursEntityToAdd = workHoursToAdd.Select(model => _mapper.MapTrainerWorkHoursModelToEntityBase(model)).ToList();
            foreach (var workHoursEntity in workHoursEntityToAdd)
            {
                _repository.AddTrainerWorkHours(workHoursEntity);
            }

            List<TrainerWorkHoursModel> rowsToUpdate = trainerWorkHours.Where(x => x.Id != 0).ToList();
            var entityRowsToUpdate = rowsToUpdate.Select(model => _mapper.MapTrainerWorkHoursModelToEntityBase(model)).ToList();
            foreach (var workHoursToUpdate in entityRowsToUpdate)
            {
                _repository.UpdateTrainerWorkHours(workHoursToUpdate);
            }

            return true;
        }


        public IEnumerable<string> GetAllClientsIdByTrainer(string trainerId)
        {
            List<string> clientsId = _repository.GetAllClientsIdByTrainer(trainerId).ToList();
            return clientsId;
        }


        public IEnumerable<string> GetActualClientsIdByTrainer(string trainerId)
        {
            List<string> clientsId = _repository.GetActualClientsIdByTrainer(trainerId).ToList();
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

            var trainersBaseByFilter = _repository.GetTrainersWithGymAndTrainingsByFilter(selectedGendersString, selectedSpecializationsString);
            List<TrainerModel> trainerModels = new List<TrainerModel>();
            foreach (var trainerBase in trainersBaseByFilter)
            {
                trainerModels.Add(_mapper.MapTrainerWithGymAndTrainingsBaseToModel(trainerBase));
            }
            return trainerModels;
        }


        public void DeleteTrainer(string id)
        {
            _repository.DeleteAllTrainingTrainerConnectionsByTrainer(id);
            _repository.DeleteTrainerWorkHoursByTrainer(id);
            _repository.DeleteTrainer(id);
            _repository.DeleteTAllTrainerWorkLicensesByTrainer(id);
        }


        public void DeleteTrainerWorkHoursByTrainer(string trainerId)
        {
            _repository.DeleteTrainerWorkHoursByTrainer(trainerId);
        }


        public bool AddTrainer(TrainerModel trainer)
        {
            var trainerEntityBase = _mapper.MapTrainerModelToEntityBase(trainer);
            bool result = _repository.AddTrainer(trainerEntityBase);
            return result;
        }


        //TrainingTrainer
        public void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId)
        {
            _repository.DeleteAllTrainingTrainerConnectionsByTrainer(trainerId);
        }


        public bool AddTrainingTrainerConnection(string trainerId, int trainingId)
        {
            bool result = _repository.AddTrainingTrainerConnection(trainerId, trainingId);
            return result;
        }


        public TrainerSpecializationsEnum GetTrainerSpecializationByTrainings(IEnumerable<int> trainingsId)
        {
            var trainingNames = new List<string>();
            foreach (var trainingId in trainingsId)
            {
                string trainingName = _repository.GetTraining(trainingId).Name;
                trainingNames.Add(trainingName);
            }

            if (trainingNames.Contains("Personal training"))
            {
                if (trainingNames.Count > 1)
                {
                    return TrainerSpecializationsEnum.universal;
                }

                return TrainerSpecializationsEnum.personal;
            }

            return TrainerSpecializationsEnum.group;
        }



        //TrainerApplication

        public int AddTrainerApplication(TrainerApplicationModel trainerApplication)
        {
            TrainerApplicationEntityBase trainerAppEntityBase =
                _mapper.MapTrainerApplicationModelToEntityBase(trainerApplication);
            int appId = _repository.AddTrainerApplication(trainerAppEntityBase);
            return appId;
        }


        public int GetTrainerApplicationsCount()
        {
            int trainerApplicationsCount = _repository.GetTrainerApplicationsCount();
            return trainerApplicationsCount;
        }


        public IEnumerable<TrainerApplicationModel> GetAllTrainerApplications()
        {
            List<TrainerApplicationModel> trainerAppModels = new List<TrainerApplicationModel>();

            var allTrainerApps = _repository.GetAllTrainerApplications();
            foreach (var trainerApp in allTrainerApps)
            {
                trainerAppModels.Add(_mapper.MapTrainerApplicationWithNamesBaseToModel(trainerApp));
            }

            return trainerAppModels;
        }


        public bool ApproveTrainerApplication(string userId)
        {
            var trainerLicense = new TrainerWorkLicenseEntityBase()
            {
                TrainerId = userId,
                ConfirmationDate = DateTime.Today
            };
            var application = _repository.GetTrainerApplicationByUser(userId);

            if (application.TrainerSubscription)
            {
                var subscription = _repository.GetUserSubscriptionsFullInfo(userId).Where(x => x.WorkAsTrainer == true)
                    .First(x => x.EndDate > DateTime.Today);

                trainerLicense.SubscriptionId = subscription.Id;
                trainerLicense.GymId = subscription.GymId;
                trainerLicense.StartDate = DateTime.Today;
                trainerLicense.EndDate = subscription.EndDate.AddDays((DateTime.Today - subscription.StartDate).TotalDays);

                _repository.UpdateUserSubscriptionDates(subscription.Id, trainerLicense.StartDate, trainerLicense.EndDate);
            }
            else
            {
                //Some logic of work with trainers Contracts area
                Random rnd = new Random();
                trainerLicense.ContractNumber = application.ContractNumber;
                trainerLicense.GymId = rnd.Next(1, 4);
                trainerLicense.StartDate = DateTime.Today;
                trainerLicense.EndDate = DateTime.Today.AddDays(256);
            }

            int licenseId = _repository.AddTrainerWorkLicense(trainerLicense);
            if (licenseId == 0)
            {
                return false;
            }

            var trainerInfo = new TrainerEntityBase()
            {
                Id = application.UserId,
                Specialization = TrainerSpecializationsEnum.personal.ToString(),
                WorkLicenseId = licenseId
            };

            bool addTrainerSucceed = _repository.AddTrainer(trainerInfo);
            if (addTrainerSucceed)
            {
                //Default type - personalTraining. If not -> Some logic of work with trainers Contracts area to get training types
                int defaultTrainingId = _repository.GetAllTrainings().First(x => x.Name == "Personal training").Id;
                _repository.AddTrainingTrainerConnection(trainerLicense.TrainerId, defaultTrainingId);

                _repository.DeleteTrainerApplication(application.Id);
            }
            else
            {
                return false;
            }

            return true;
        }


        public TrainerApplicationModel GetTrainerApplicationByUser(string userId)
        {
            var applicationEntity = _repository.GetTrainerApplicationWithNamesByUser(userId);
            if (applicationEntity is null)
            {
                return null;
            }
            TrainerApplicationModel trainerAppModel = _mapper.MapTrainerApplicationWithNamesBaseToModel(applicationEntity);
            return trainerAppModel;
        }


        public void DeleteTrainerApplication(int applicationId)
        {
            _repository.DeleteTrainerApplication(applicationId);
        }

        //Work License

        public TrainerWorkLicenseModel GetTrainerWorkLicenseByTrainer(string userId)
        {
            var licenseEntity = _repository.GetTrainerWorkLicenseByTrainer(userId);
            if (licenseEntity != null)
            {
                var licenseModel = _mapper.MapTrainerWorkLicenseEntityBaseToModel(licenseEntity);
                return licenseModel;
            }
            return null;
        }

    }
}
