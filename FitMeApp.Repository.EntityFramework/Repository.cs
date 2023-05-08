using FitMeApp.Repository.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities.JoinEntityBase;
using FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Repository.EntityFramework
{
    public sealed class Repository : IRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<Repository> _logger;
        public Repository(ApplicationDbContext context, ILogger<Repository> logger)
        {
            _context = context;
            _logger = logger;
        }


        public IEnumerable<GymEntityBase> GetAllGyms()
        {
            var gyms = _context.Gyms.ToList();
            return gyms;
        }


        public IEnumerable<GymWithGalleryBase> GetAllGymsWithGallery()
        {
            var gymsWithGallery = (from gym in _context.Gyms
                                   join image in _context.GymImages
                                       on gym.Id equals image.GymId
                                   select new
                                   {
                                       Id = gym.Id,
                                       Name = gym.Name,
                                       Address = gym.Address,
                                       Phone = gym.Phone,
                                       GymImagePath = image.ImagePath
                                   }).ToList();

            List<GymWithGalleryBase> gyms = new List<GymWithGalleryBase>();

            foreach (var gymEntity in gymsWithGallery)
            {
                if (!gyms.Select(x => x.Id).Contains(gymEntity.Id))
                {
                    List<string> gymImagePaths = gymsWithGallery
                        .Where(x => x.Id == gymEntity.Id)
                        .Select(x => x.GymImagePath)
                        .ToList();

                    gyms.Add(new GymWithGalleryBase()
                    {
                        Id = gymEntity.Id,
                        Name = gymEntity.Name,
                        Address = gymEntity.Address,
                        Phone = gymEntity.Phone,
                        GymImagePaths = gymImagePaths
                    });
                }
            }

            return gyms;
        }


        public GymEntityBase GetGym(int id)
        {
            GymEntity gym = _context.Gyms.FirstOrDefault(x => x.Id == id);
            return gym;
        }


        public GymEntityBase AddGym(GymEntityBase gym)
        {
            _context.Gyms.Add(new GymEntity()
            {
                Name = gym.Name,
                Address = gym.Address,
                Phone = gym.Phone
            });
            _context.SaveChanges();
            return gym;
        }


        public void UpdateGym(int id, GymEntityBase newGymData)
        {
            GymEntity gym = _context.Gyms.FirstOrDefault(x => x.Id == id);

            if (gym != null)
            {
                gym.Name = newGymData.Name;
                gym.Address = newGymData.Address;
                gym.Phone = newGymData.Phone;
            }

            _context.SaveChanges();
        }


        public void DeleteGym(int id)
        {
            GymEntity gym = _context.Gyms.FirstOrDefault(x => x.Id == id);
            if (gym != null)
            {
                _context.Gyms.Remove(gym);
                _context.SaveChanges();
            }
        }


        public IEnumerable<GymWorkHoursEntityBase> GetWorkHoursByGym(int gymId)
        {
            var gymWorkHours = _context.GymWorkHours
                .Where(x => x.GymId == gymId)
                .OrderBy(x => x.DayOfWeekNumber)
                .ToList();
            return gymWorkHours;
        }


        public int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek)
        {
            int gymWorkHoursId = _context.GymWorkHours
                .Where(x => x.GymId == gymId)
                .Where(x => x.DayOfWeekNumber == dayOfWeek)
                .Select(x => x.Id)
                .FirstOrDefault();

            return gymWorkHoursId;
        }


        //Trainers//

        public IEnumerable<TrainerEntityBase> GetAllTrainers()
        {
            var trainers = _context.Trainers.ToList();
            return trainers;
        }


        public TrainerEntityBase GetTrainer(string id)
        {
            var trainer = _context.Trainers.FirstOrDefault(x => x.Id == id);
            return trainer;
        }


        public bool AddTrainer(TrainerEntityBase trainer)
        {
            _context.Trainers.Add(new TrainerEntity()
            {
                Id = trainer.Id,
                Specialization = trainer.Specialization,
                WorkLicenseId = trainer.WorkLicenseId
            });

            int addedRowCount = _context.SaveChanges();
            return addedRowCount > 0;
        }


        public void UpdateTrainer(TrainerEntityBase newTrainerData)
        {
            var trainer = _context.Trainers
                .First(x => x.Id == newTrainerData.Id);

            trainer.Specialization = newTrainerData.Specialization;
            trainer.WorkLicenseId = newTrainerData.WorkLicenseId;

            _context.SaveChanges();
        }


        public void DeleteTrainer(string id)
        {
            var trainer = _context.Trainers
                .First(x => x.Id == id);

            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
            }
        }


        public int GetGymIdByTrainer(string trainerId)
        {
            int gymId = 0;
            var license = _context.TrainerWorkLicenses
                .Where(x => x.TrainerId == trainerId)
                .OrderBy(x => x.EndDate)
                .FirstOrDefault();

            if (license != null)
            {
                gymId = license.GymId;
            }

            return gymId;
        }


        public IEnumerable<TrainerWorkHoursWithDayBase> GetWorkHoursByTrainer(string trainerId)
        {
            var trainerWorkHoursWithDays = (from trainerWorkHours in _context.TrainerWorkHours
                                            join gymWorkHours in _context.GymWorkHours
                                            on trainerWorkHours.GymWorkHoursId equals gymWorkHours.Id
                                            where trainerWorkHours.TrainerId == trainerId
                                            select new TrainerWorkHoursWithDayBase()
                                            {
                                                Id = trainerWorkHours.Id,
                                                TrainerId = trainerWorkHours.TrainerId,
                                                StartTime = trainerWorkHours.StartTime,
                                                EndTime = trainerWorkHours.EndTime,
                                                GymWorkHoursId = trainerWorkHours.GymWorkHoursId,
                                                DayName = gymWorkHours.DayOfWeekNumber
                                            })
                                            .OrderBy(x => x.DayName)
                                            .ToList();

            return trainerWorkHoursWithDays;
        }


        public IEnumerable<string> GetAllClientsIdByTrainer(string trainerId)
        {
            List<string> clientsId = _context.Events
                .Where(x => x.TrainerId == trainerId)
                .Select(x => x.UserId)
                .Distinct()
                .ToList();

            return clientsId;
        }


        public IEnumerable<string> GetActualClientsIdByTrainer(string trainerId)
        {
            List<string> clientsId = _context.Events
                .Where(x => x.TrainerId == trainerId)
                .Where(x => x.Date >= DateTime.Today)
                .Select(x => x.UserId)
                .Distinct()
                .ToList();

            return clientsId;
        }


        public void DeleteTrainerWorkHoursByTrainer(string trainerId)
        {
            var trainerWorkHours = _context.TrainerWorkHours
                .Where(x => x.TrainerId == trainerId)
                .ToList();

            if (trainerWorkHours.Count > 0)
            {
                _context.TrainerWorkHours.RemoveRange(trainerWorkHours);
                _context.SaveChanges();
            }
        }


        public IEnumerable<int> GetAvailableToApplyTrainingTimeByTrainer(string trainerId, DateTime date)
        {
            var trainerWorkHoursJoin = (from trainerWorkHours in _context.TrainerWorkHours
                                        join gymWorkHours in _context.GymWorkHours
                                        on trainerWorkHours.GymWorkHoursId equals gymWorkHours.Id
                                        where trainerWorkHours.TrainerId == trainerId
                                        where gymWorkHours.DayOfWeekNumber == date.DayOfWeek
                                        select new
                                        {
                                            StartTime = trainerWorkHours.StartTime,
                                            EndTime = trainerWorkHours.EndTime
                                        })
                                    .FirstOrDefault();

            if (trainerWorkHoursJoin is null)
            {
                return new List<int>();
            }

            List<int> workTimeScale = new List<int>();
            for (int timeInMinutes = trainerWorkHoursJoin.StartTime; timeInMinutes < trainerWorkHoursJoin.EndTime; timeInMinutes = (timeInMinutes + 30))
            {
                workTimeScale.Add(timeInMinutes);
            }


            var existingEventsStartEnd = _context.Events
                .Where(x => x.TrainerId == trainerId)
                .Where(x => x.Date == date)
                .Select(x => new { StartEvent = x.StartTime, EndEvent = x.EndTime })
                .ToList();

            List<int> occupiedTime = new List<int>();

            //Time at 30 minutes before events is not available to applying training
            foreach (var eventTime in existingEventsStartEnd)
            {
                for (int timeInMinutes = (eventTime.StartEvent - 30); timeInMinutes < eventTime.EndEvent; timeInMinutes = (timeInMinutes + 30))
                {
                    occupiedTime.Add(timeInMinutes);
                }
            }

            List<int> availableTime = workTimeScale
                .Except(occupiedTime)
                .ToList();

            return availableTime;
        }


        public int AddTrainerWorkLicense(TrainerWorkLicenseEntityBase license)
        {
            TrainerWorkLicenseEntity licenseEntity = new TrainerWorkLicenseEntity()
            {
                TrainerId = license.TrainerId,
                SubscriptionId = license.SubscriptionId,
                ContractNumber = license.ContractNumber,
                GymId = license.GymId,
                StartDate = license.StartDate,
                EndDate = license.EndDate,
                ConfirmationDate = license.ConfirmationDate
            };

            _context.TrainerWorkLicenses.Add(licenseEntity);
            _context.SaveChanges();
            return licenseEntity.Id;
        }


        public void DeleteAllTrainerWorkLicensesByTrainer(string trainerId)
        {
            var trainerWorkLicenses = _context.TrainerWorkLicenses
                .Where(x => x.TrainerId == trainerId)
                .ToList();

            foreach (var license in trainerWorkLicenses)
            {
                _context.Remove(license);
            }

            _context.SaveChanges();
        }


        public void DeleteTrainerWorkLicense(int licenseId)
        {
            var license = _context.TrainerWorkLicenses.FirstOrDefault(x => x.Id == licenseId);
            if (license != null)
            {
                _context.Remove(license);
                _context.SaveChanges();
            }
        }


        public TrainerWorkLicenseEntityBase GetTrainerWorkLicense(int licenseId)
        {
            var license = _context.TrainerWorkLicenses
                .FirstOrDefault(x => x.Id == licenseId);

            return license;
        }

        public TrainerWorkLicenseEntityBase GetTrainerWorkLicenseByTrainer(string trainerId)
        {
            var licenses = _context.TrainerWorkLicenses
                .FirstOrDefault(x => x.TrainerId == trainerId);
            return licenses;
        }


        public IEnumerable<TrainerWorkLicenseEntityBase> GetAllTrainerWorkLicense()
        {
            var licenses = _context.TrainerWorkLicenses.ToList();
            return licenses;
        }




        //Trainer applications
        public IEnumerable<TrainerApplicationWithNamesBase> GetAllTrainerApplications()
        {
            var trainerApplications = (from trainerApp in _context.TrainerApplications
                                       join user in _context.Users
                                           on trainerApp.UserId equals user.Id
                                       select new TrainerApplicationWithNamesBase()
                                       {
                                           Id = trainerApp.Id,
                                           UserId = trainerApp.UserId,
                                           UserFirstName = user.FirstName,
                                           UserLastName = user.LastName,
                                           SubscriptionId = trainerApp.SubscriptionId,
                                           ContractNumber = trainerApp.ContractNumber,
                                           GymId = trainerApp.GymId,
                                           StartDate = trainerApp.StartDate,
                                           EndDate = trainerApp.EndDate,
                                           ApplyingDate = trainerApp.ApplyingDate
                                       }).ToList();

            return trainerApplications;
        }


        public TrainerApplicationWithNamesBase GetTrainerApplicationWithNamesByUser(string userId)
        {
            var trainerApplication = (from trainerApp in _context.TrainerApplications
                                      join user in _context.Users
                                          on trainerApp.UserId equals user.Id
                                      where trainerApp.UserId == userId
                                      select new TrainerApplicationWithNamesBase()
                                      {
                                          Id = trainerApp.Id,
                                          UserId = trainerApp.UserId,
                                          UserFirstName = user.FirstName,
                                          UserLastName = user.LastName,
                                          SubscriptionId = trainerApp.SubscriptionId,
                                          ContractNumber = trainerApp.ContractNumber,
                                          GymId = trainerApp.GymId,
                                          StartDate = trainerApp.StartDate,
                                          EndDate = trainerApp.EndDate,
                                          ApplyingDate = trainerApp.ApplyingDate
                                      }).FirstOrDefault();

            return trainerApplication;
        }


        public TrainerApplicationEntityBase GetTrainerApplicationByUser(string userId)
        {
            var trainerApplication = _context.TrainerApplications.FirstOrDefault(x => x.UserId == userId);
            return trainerApplication;
        }


        public int AddTrainerApplication(TrainerApplicationEntityBase trainerApplication)
        {
            TrainerApplicationEntity trainerAppEntity = new TrainerApplicationEntity()
            {
                UserId = trainerApplication.UserId,
                SubscriptionId = trainerApplication.SubscriptionId,
                ContractNumber = trainerApplication.ContractNumber,
                GymId = trainerApplication.GymId,
                StartDate = trainerApplication.StartDate,
                EndDate = trainerApplication.EndDate,
                ApplyingDate = trainerApplication.ApplyingDate
            };
            _context.TrainerApplications.Add(trainerAppEntity);
            _context.SaveChanges();

            return trainerAppEntity.Id;
        }

        public int GetTrainerApplicationsCount()
        {
            int trainerAppCount = _context.TrainerApplications.Count();
            return trainerAppCount;
        }

        public void DeleteTrainerApplication(int appId)
        {
            var application = _context.TrainerApplications.FirstOrDefault(x => x.Id == appId);
            if (application != null)
            {
                _context.Remove(application);
                _context.SaveChanges();
            }
        }


        //Edit Trainer WorkHours methods
        public void AddTrainerWorkHours(TrainerWorkHoursEntityBase workHoursBase)
        {
            if (_context.TrainerWorkHours.Find(workHoursBase.Id) == null)
            {
                _context.TrainerWorkHours.Add(new TrainerWorkHoursEntity()
                {
                    TrainerId = workHoursBase.TrainerId,
                    StartTime = workHoursBase.StartTime,
                    EndTime = workHoursBase.EndTime,
                    GymWorkHoursId = workHoursBase.GymWorkHoursId
                });

                _context.SaveChanges();
            }
        }


        public void DeleteTrainerWorkHours(int workHoursId)
        {
            var trainerWorkHoursForCurrentDay = _context.TrainerWorkHours.FirstOrDefault(x => x.Id == workHoursId);
            if (trainerWorkHoursForCurrentDay != null)
            {
                _context.Remove(trainerWorkHoursForCurrentDay);
                _context.SaveChanges();
            }
        }


        public void UpdateTrainerWorkHours(TrainerWorkHoursEntityBase newTrainerWorkHours)
        {
            TrainerWorkHoursEntity workHoursEntity =
                _context.TrainerWorkHours.FirstOrDefault(x => x.Id == newTrainerWorkHours.Id);
            if (workHoursEntity != null)
            {
                workHoursEntity.StartTime = newTrainerWorkHours.StartTime;
                workHoursEntity.EndTime = newTrainerWorkHours.EndTime;
                _context.SaveChanges();
            }
        }


        public IEnumerable<int> GerAllTrainerWorkHoursId(string trainerId)
        {
            List<int> allTrainerWorkHoursId = _context.TrainerWorkHours
                .Where(x => x.TrainerId == trainerId)
                .Select(x => x.Id)
                .ToList();

            return allTrainerWorkHoursId;
        }



        //Trainings
        public IEnumerable<TrainingEntityBase> GetAllTrainings()
        {
            var trainings = _context.Trainings.ToList();
            return trainings;
        }


        public TrainingEntityBase GetTraining(int id)
        {
            var training = _context.Trainings.FirstOrDefault(x => x.Id == id);
            if (training is null)
            {
                throw new ArgumentException($"training by id {id} not found");
            }
            return training;
        }


        public TrainingEntityBase AddTraining(TrainingEntityBase training)
        {
            if (training == null)
            {
                throw new ArgumentNullException(nameof(training));
            }

            _context.Add(new TrainingEntity()
            {
                Name = training.Name,
                Description = training.Description
            });
            _context.SaveChanges();
            return training;
        }


        public void UpdateTraining(int id, TrainingEntityBase newGroupClassData)
        {
            if (newGroupClassData == null)
            {
                throw new ArgumentNullException(nameof(newGroupClassData));
            }

            var groupClass = _context.Trainings.FirstOrDefault(x => x.Id == id);
            if (groupClass != null)
            {
                groupClass.Name = newGroupClassData.Name;
                groupClass.Description = newGroupClassData.Description;
                _context.SaveChanges();
            }
        }


        public void DeleteTraining(int id)
        {
            var groupClass = _context.Trainings
                .FirstOrDefault(x => x.Id == id);

            if (groupClass != null)
            {
                _context.Trainings.Remove(groupClass);
                _context.SaveChanges();
            }

        }


        //Trainer-Trainings
        public IEnumerable<int> GetAllTrainingIdsByTrainer(string trainerId)
        {
            var trainingsIds = _context.TrainingTrainer
                .Where(x => x.TrainerId == trainerId)
                .Select(x => x.TrainingId)
                .ToList();
            return trainingsIds;
        }


        public void DeleteTrainingTrainerConnection(string trainerId, int trainingToDeleteId)
        {
            var trainingTrainersConnection = _context.TrainingTrainer
                .Where(x => x.TrainerId == trainerId)
                .FirstOrDefault(x => x.TrainingId == trainingToDeleteId);

            if (trainingTrainersConnection != null)
            {
                _context.TrainingTrainer.Remove(trainingTrainersConnection);
                _context.SaveChanges();
            }
        }


        public bool AddTrainingTrainerConnection(string trainerId, int trainingToAddId)
        {
            _context.TrainingTrainer.Add(new TrainingTrainerEntity()
            {
                TrainerId = trainerId,
                TrainingId = trainingToAddId
            });

            int addedCount = _context.SaveChanges();
            return addedCount > 0;
        }


        public void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId)
        {
            var allTrainingsByTrainer = _context.TrainingTrainer
                .Where(x => x.TrainerId == trainerId)
                .ToList();

            if (allTrainingsByTrainer.Count > 0)
            {
                _context.TrainingTrainer.RemoveRange(allTrainingsByTrainer);
                _context.SaveChanges();
            }
        }


        //Gym - Trainer - Trainings connection

        public GymWithTrainersAndTrainings GetGymWithTrainersAndTrainings(int gymId)
        {
            var gymTrainerTrainingJoin = (from gymDb in _context.Gyms
                                          join license in _context.TrainerWorkLicenses
                                          on gymDb.Id equals license.GymId
                                          join trainer in _context.Trainers
                                          on license.TrainerId equals trainer.Id
                                          join user in _context.Users
                                          on trainer.Id equals user.Id
                                          join trainingTrainer in _context.TrainingTrainer
                                          on trainer.Id equals trainingTrainer.TrainerId
                                          join training in _context.Trainings
                                          on trainingTrainer.TrainingId equals training.Id
                                          join image in _context.GymImages
                                              on gymDb.Id equals image.GymId
                                          where gymDb.Id == gymId
                                          select new
                                          {
                                              GymId = gymDb.Id,
                                              GymName = gymDb.Name,
                                              GymAddress = gymDb.Address,
                                              GymPhone = gymDb.Phone,
                                              GymImagePath = image.ImagePath,
                                              TrainerId = trainer.Id,
                                              TrainerFirstName = user.FirstName,
                                              TrainerLastName = user.LastName,
                                              TrainerGender = user.Gender,
                                              TrainerAvatarPath = user.AvatarPath,
                                              TrainerSpecialization = trainer.Specialization,
                                              TrainingId = training.Id,
                                              TrainingName = training.Name,
                                              TrainingDescription = training.Description
                                          }).ToList();

            GymWithTrainersAndTrainings gym = new GymWithTrainersAndTrainings();
            var gymInfo = gymTrainerTrainingJoin.First();
            gym.Id = gymInfo.GymId;
            gym.Name = gymInfo.GymName;
            gym.Phone = gymInfo.GymPhone;
            gym.Address = gymInfo.GymAddress;

            List<string> gymImagePaths = new List<string>();
            foreach (var item in gymTrainerTrainingJoin)
            {
                if (!gymImagePaths.Contains(item.GymImagePath))
                {
                    gymImagePaths.Add(item.GymImagePath);
                }
            }
            gym.GymImagePaths = gymImagePaths;


            var trainers = new List<TrainerWithGymAndTrainingsBase>();
            List<string> addedTrainersId = new List<string>();
            foreach (var item in gymTrainerTrainingJoin)
            {
                var training = new TrainingEntityBase()
                {
                    Id = item.TrainingId,
                    Name = item.TrainingName,
                    Description = item.TrainingDescription
                };

                if (!addedTrainersId.Contains(item.TrainerId))
                {
                    var trainings = new List<TrainingEntityBase>();
                    trainings.Add(training);

                    trainers.Add(new TrainerWithGymAndTrainingsBase()
                    {
                        Id = item.TrainerId,
                        FirstName = item.TrainerFirstName,
                        LastName = item.TrainerLastName,
                        Gender = item.TrainerGender,
                        AvatarPath = item.TrainerAvatarPath,
                        Specialization = item.TrainerSpecialization,
                        Trainings = trainings
                    });
                    addedTrainersId.Add(item.TrainerId);
                }
                else
                {
                    var currentTrainer = trainers.First(x => x.Id == item.TrainerId);
                    var currentTrainerTrainings = currentTrainer.Trainings.ToList();
                    currentTrainerTrainings.Add(training);
                    trainers.First(x => x.Id == item.TrainerId).Trainings = currentTrainerTrainings;
                }
            }

            gym.Trainers = trainers;
            return gym;
        }



        public IEnumerable<TrainerWithGymAndTrainingsBase> GetAllTrainersWithGymAndTrainings()
        {
            var allTrainersGymTrainingsJoin = (from trainer in _context.Trainers
                                               join user in _context.Users
                                               on trainer.Id equals user.Id
                                               join license in _context.TrainerWorkLicenses
                                               on trainer.Id equals license.TrainerId
                                               join gym in _context.Gyms
                                               on license.GymId equals gym.Id
                                               join trainingTrainer in _context.TrainingTrainer
                                               on trainer.Id equals trainingTrainer.TrainerId
                                               join training in _context.Trainings
                                               on trainingTrainer.TrainingId equals training.Id
                                               select new TrainerWithGymAndTrainingsJoin()
                                               {
                                                   TrainerId = trainer.Id,
                                                   FirstName = user.FirstName,
                                                   LastName = user.LastName,
                                                   Gender = user.Gender,
                                                   AvatarPath = user.AvatarPath,
                                                   Specialization = trainer.Specialization,
                                                   GymId = gym.Id,
                                                   GymName = gym.Name,
                                                   GymAddress = gym.Address,
                                                   TrainingId = training.Id,
                                                   TrainingName = training.Name
                                               }).OrderBy(x => x.TrainerId).ToList();

            List<TrainerWithGymAndTrainingsBase> trainersWithGymAndTrainings = ConvertJoinResultToTrainerWithGymAndTrainingsBase(allTrainersGymTrainingsJoin);

            return trainersWithGymAndTrainings;
        }


        private List<TrainerWithGymAndTrainingsBase> ConvertJoinResultToTrainerWithGymAndTrainingsBase(IEnumerable<TrainerWithGymAndTrainingsJoin> trainersGymsTrainingsJoins)
        {
            List<TrainerWithGymAndTrainingsBase> trainersWithGymAndTrainings = new List<TrainerWithGymAndTrainingsBase>();
            List<string> trainersId = new List<string>();
            var trainerWithGymAndTrainings = new TrainerWithGymAndTrainingsBase();

            foreach (var item in trainersGymsTrainingsJoins)
            {
                if (!trainersId.Contains(item.TrainerId))
                {
                    List<TrainingEntityBase> trainings = new List<TrainingEntityBase>();
                    trainings.Add(new TrainingEntityBase()
                    {
                        Id = item.TrainingId,
                        Name = item.TrainingName
                    });

                    trainerWithGymAndTrainings = new TrainerWithGymAndTrainingsBase()
                    {
                        Id = item.TrainerId,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Gender = item.Gender,
                        AvatarPath = item.AvatarPath,
                        Specialization = item.Specialization,
                        Gym = new GymEntityBase()
                        {
                            Id = item.GymId,
                            Name = item.GymName,
                            Address = item.GymAddress
                        },
                        Trainings = trainings
                    };

                    trainersWithGymAndTrainings.Add(trainerWithGymAndTrainings);
                    trainersId.Add(item.TrainerId);
                }
                else
                {
                    List<TrainingEntityBase> currentTrainerTrainings = trainerWithGymAndTrainings.Trainings.ToList();
                    currentTrainerTrainings.Add(new TrainingEntityBase()
                    {
                        Id = item.TrainingId,
                        Name = item.TrainingName
                    });
                    trainerWithGymAndTrainings.Trainings = currentTrainerTrainings;
                }
            }

            return trainersWithGymAndTrainings;
        }



        public TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(string trainerId)
        {
            var trainerGymTrainingJoin = (from trainer in _context.Trainers
                                          join user in _context.Users
                                          on trainer.Id equals user.Id
                                          join license in _context.TrainerWorkLicenses
                                          on trainer.Id equals license.TrainerId
                                          join gym in _context.Gyms
                                          on license.GymId equals gym.Id
                                          join trainingTrainer in _context.TrainingTrainer
                                          on trainer.Id equals trainingTrainer.TrainerId
                                          join training in _context.Trainings
                                          on trainingTrainer.TrainingId equals training.Id
                                          where trainer.Id == trainerId
                                          select new
                                          {
                                              Id = trainer.Id,
                                              FirstName = user.FirstName,
                                              LastName = user.LastName,
                                              Specialization = trainer.Specialization,
                                              Gender = user.Gender,
                                              AvatarPath = user.AvatarPath,
                                              GymId = gym.Id,
                                              GymName = gym.Name,
                                              GymAddress = gym.Address,
                                              TrainingId = training.Id,
                                              TrainingName = training.Name
                                          }).ToList();

            if (trainerGymTrainingJoin.Count == 0)
            {
                return null;
            }

            List<TrainingEntity> trainingEntities = new List<TrainingEntity>();
            foreach (var item in trainerGymTrainingJoin)
            {
                trainingEntities.Add(new TrainingEntity()
                {
                    Id = item.TrainingId,
                    Name = item.TrainingName
                });
            }

            var trainerEntity = trainerGymTrainingJoin.First();

            TrainerWithGymAndTrainingsBase trainerWithGymAndTraining = new TrainerWithGymAndTrainingsBase()
            {
                Id = trainerEntity.Id,
                FirstName = trainerEntity.FirstName,
                LastName = trainerEntity.LastName,
                Specialization = trainerEntity.Specialization,
                Gender = trainerEntity.Gender,
                AvatarPath = trainerEntity.AvatarPath,
                Gym = new GymEntity()
                {
                    Id = trainerEntity.GymId,
                    Name = trainerEntity.GymName,
                    Address = trainerEntity.GymAddress
                },
                Trainings = trainingEntities
            };

            return trainerWithGymAndTraining;
        }



        public TrainingWithTrainerAndGymBase GetTrainingWithTrainersAndGyms(int trainingId)
        {
            var trainingTrainerGymJoin = (from training in _context.Trainings
                                          join trainingTrainer in _context.TrainingTrainer
                                              on training.Id equals trainingTrainer.TrainingId
                                          join trainer in _context.Users
                                              on trainingTrainer.TrainerId equals trainer.Id
                                          join workLicense in _context.TrainerWorkLicenses
                                              on trainer.Id equals workLicense.TrainerId
                                          join gym in _context.Gyms
                                              on workLicense.GymId equals gym.Id
                                          where training.Id == trainingId
                                          select new
                                          {
                                              Id = training.Id,
                                              Name = training.Name,
                                              Description = training.Description,
                                              TrainerId = trainer.Id,
                                              TrainerFirstName = trainer.FirstName,
                                              TrainerLastName = trainer.LastName,
                                              TrainerAvatar = trainer.AvatarPath,
                                              GymId = workLicense.GymId,
                                              GymName = gym.Name,
                                              GymAdress = gym.Address
                                          }).ToList();

            if (trainingTrainerGymJoin.Count == 0)
            {
                return new TrainingWithTrainerAndGymBase();
            }

            var gyms = new List<GymEntityBase>();
            var trainers = new List<User>();

            foreach (var item in trainingTrainerGymJoin)
            {
                gyms.Add(new GymEntityBase()
                {
                    Id = item.GymId,
                    Name = item.GymName,
                    Address = item.GymAdress
                });

                trainers.Add(new User()
                {
                    Id = item.TrainerId,
                    FirstName = item.TrainerFirstName,
                    LastName = item.TrainerLastName,
                    AvatarPath = item.TrainerAvatar
                });
            }

            var trainingWithTrainersAndGyms = new TrainingWithTrainerAndGymBase()
            {
                Id = trainingTrainerGymJoin.First().Id,
                Name = trainingTrainerGymJoin.First().Name,
                Description = trainingTrainerGymJoin.First().Description,
                Gyms = gyms,
                Trainers = trainers
            };

            return trainingWithTrainersAndGyms;
        }



        //Filters

        public IEnumerable<GymWithGalleryBase> GetGymsByTrainings(IEnumerable<int> trainingsId)
        {
            var gymsByTrainings = (from trainingTrainer in _context.TrainingTrainer
                                   join license in _context.TrainerWorkLicenses
                                       on trainingTrainer.TrainerId equals license.TrainerId
                                   join gymDb in _context.Gyms
                                   on license.GymId equals gymDb.Id
                                   join image in _context.GymImages
                                       on gymDb.Id equals image.GymId
                                   where trainingsId.Contains(trainingTrainer.TrainingId)
                                   select new
                                   {
                                       Id = gymDb.Id,
                                       Name = gymDb.Name,
                                       Address = gymDb.Address,
                                       Phone = gymDb.Phone,
                                       GymImagePaths = image.ImagePath
                                   }).ToList().Distinct();

            List<GymWithGalleryBase> gyms = new List<GymWithGalleryBase>();
            foreach (var gym in gymsByTrainings)
            {
                if (!gyms.Select(x => x.Id).Contains(gym.Id))
                {
                    var images = gymsByTrainings
                        .Where(x => x.Id == gym.Id)
                        .Select(x => x.GymImagePaths)
                        .ToList();

                    gyms.Add(new GymWithGalleryBase()
                    {
                        Id = gym.Id,
                        Name = gym.Name,
                        Address = gym.Address,
                        Phone = gym.Phone,
                        GymImagePaths = images
                    });
                }
            }

            return gyms;
        }


        public IEnumerable<SubscriptionPriceBase> GetSubscriptionsForVisitorsByGymByFilter(int gymId, IEnumerable<int> periods, bool groupTraining, bool dietMonitoring)
        {
            var subscriptions = (from gymSubscription in _context.GymSubscriptions
                                 join subscription in _context.Subscriptions
                                 on gymSubscription.SubscriptionId equals subscription.Id
                                 where gymSubscription.GymId == gymId
                                 where periods.Contains(subscription.ValidDays)
                                 where subscription.GroupTraining == groupTraining
                                 where subscription.DietMonitoring == dietMonitoring
                                 where subscription.WorkAsTrainer == false
                                 select new SubscriptionPriceBase()
                                 {
                                     Id = subscription.Id,
                                     GymId = gymSubscription.GymId,
                                     ValidDays = subscription.ValidDays,
                                     GroupTraining = subscription.GroupTraining,
                                     DietMonitoring = subscription.DietMonitoring,
                                     Price = gymSubscription.Price
                                 }).ToList();

            return subscriptions;
        }


        public IEnumerable<TrainerWithGymAndTrainingsBase> GetTrainersWithGymAndTrainingsByFilter(IEnumerable<string> selectedGenders, IEnumerable<string> selectedSpecializations)
        {
            var trainersGymTrainingsByFilterJoin = (from trainer in _context.Trainers
                                                    join user in _context.Users
                                                    on trainer.Id equals user.Id
                                                    join license in _context.TrainerWorkLicenses
                                                    on trainer.Id equals license.TrainerId
                                                    join gym in _context.Gyms
                                                    on license.GymId equals gym.Id
                                                    join trainingTrainer in _context.TrainingTrainer
                                                    on trainer.Id equals trainingTrainer.TrainerId
                                                    join training in _context.Trainings
                                                    on trainingTrainer.TrainingId equals training.Id
                                                    where selectedGenders.Contains(user.Gender)
                                                    where selectedSpecializations.Contains(trainer.Specialization)
                                                    select new TrainerWithGymAndTrainingsJoin()
                                                    {
                                                        TrainerId = trainer.Id,
                                                        FirstName = user.FirstName,
                                                        LastName = user.LastName,
                                                        Gender = user.Gender,
                                                        AvatarPath = user.AvatarPath,
                                                        Specialization = trainer.Specialization,
                                                        GymId = gym.Id,
                                                        GymName = gym.Name,
                                                        GymAddress = gym.Address,
                                                        TrainingId = training.Id,
                                                        TrainingName = training.Name
                                                    }).OrderBy(x => x.TrainerId).ToList();

            List<TrainerWithGymAndTrainingsBase> trainersWithGymAndTrainings = ConvertJoinResultToTrainerWithGymAndTrainingsBase(trainersGymTrainingsByFilterJoin);

            return trainersWithGymAndTrainings;
        }


        // Subscribtions

        public IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsForVisitorsByGym(int gymId)
        {
            var subscriptions = (from gymSubscription in _context.GymSubscriptions
                                 join subscription in _context.Subscriptions
                                 on gymSubscription.SubscriptionId equals subscription.Id
                                 where gymSubscription.GymId == gymId
                                 where subscription.WorkAsTrainer == false
                                 select new SubscriptionPriceBase()
                                 {
                                     Id = subscription.Id,
                                     GymId = gymSubscription.GymId,
                                     ValidDays = subscription.ValidDays,
                                     GroupTraining = subscription.GroupTraining,
                                     DietMonitoring = subscription.DietMonitoring,
                                     Price = gymSubscription.Price
                                 }).ToList();

            return subscriptions;
        }


        public IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsForTrainersByGym(int gymId)
        {
            var subscriptions = (from gymSubscription in _context.GymSubscriptions
                                 join subscription in _context.Subscriptions
                                     on gymSubscription.SubscriptionId equals subscription.Id
                                 where gymSubscription.GymId == gymId
                                 where subscription.WorkAsTrainer == true
                                 select new SubscriptionPriceBase()
                                 {
                                     Id = subscription.Id,
                                     GymId = gymSubscription.GymId,
                                     ValidDays = subscription.ValidDays,
                                     GroupTraining = subscription.GroupTraining,
                                     DietMonitoring = subscription.DietMonitoring,
                                     WorkAsTrainer = subscription.WorkAsTrainer,
                                     Price = gymSubscription.Price
                                 }).ToList();

            return subscriptions;
        }



        public IEnumerable<int> GetAllSubscriptionPeriods()
        {
            List<int> allSubscriptionPeriods = new List<int>();
            var subscriptions = _context.Subscriptions;

            foreach (var subscription in subscriptions)
            {
                allSubscriptionPeriods.Add(subscription.ValidDays);
                allSubscriptionPeriods = allSubscriptionPeriods.Distinct().ToList();
            }

            return allSubscriptionPeriods;
        }


        public int GetSubscriptionPeriod(int subscriptionId)
        {
            int subscriptionPeriod = _context.Subscriptions
                .First(x => x.Id == subscriptionId)
                .ValidDays;

            return subscriptionPeriod;
        }


        public SubscriptionPriceBase GetSubscriptionWithPriceByGym(int subscriptionId, int gymId)
        {
            var subscriptionWithPrice = (from gymSubscription in _context.GymSubscriptions
                                         join subscription in _context.Subscriptions
                                         on gymSubscription.SubscriptionId equals subscription.Id
                                         where gymSubscription.GymId == gymId
                                         where subscription.Id == subscriptionId
                                         select new SubscriptionPriceBase()
                                         {
                                             Id = subscription.Id,
                                             GymId = gymSubscription.GymId,
                                             ValidDays = subscription.ValidDays,
                                             GroupTraining = subscription.GroupTraining,
                                             DietMonitoring = subscription.DietMonitoring,
                                             WorkAsTrainer = subscription.WorkAsTrainer,
                                             Price = gymSubscription.Price
                                         }).First();

            return subscriptionWithPrice;
        }


        public void DeleteUserSubscription(int userSubscriptionId)
        {
            var userSubscription = _context.UserSubscriptions.FirstOrDefault(x => x.Id == userSubscriptionId);
            if (userSubscription != null)
            {
                _context.Remove(userSubscription);
                _context.SaveChanges();
            }
        }


        //UserSubscriptions
        public int AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate)
        {
            int gymSubscriptionId = _context.GymSubscriptions
                .First(x => x.GymId == gymId && x.SubscriptionId == subscriptionId)
                .Id;

            int subscriptionPeriod = GetSubscriptionPeriod(subscriptionId);

            UserSubscriptionEntity userSubscription = new UserSubscriptionEntity()
            {
                UserId = userId,
                GymSubscriptionId = gymSubscriptionId,
                StartDate = startDate,
                EndDate = startDate.AddDays(subscriptionPeriod)

            };
            _context.UserSubscriptions.Add(userSubscription);
            _context.SaveChanges();

            return userSubscription.Id;
        }


        public IEnumerable<UserSubscriptionFullInfoBase> GetUserSubscriptionsFullInfo(string userId)
        {
            var userSubscriptionsFullInfo = (from userSubscr in _context.UserSubscriptions
                                             join gymSubscr in _context.GymSubscriptions
                                             on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                             join subscr in _context.Subscriptions
                                             on gymSubscr.SubscriptionId equals subscr.Id
                                             join gym in _context.Gyms
                                             on gymSubscr.GymId equals gym.Id
                                             where userSubscr.UserId == userId
                                             select new UserSubscriptionFullInfoBase()
                                             {
                                                 Id = userSubscr.Id,
                                                 UserId = userSubscr.UserId,
                                                 GymId = gym.Id,
                                                 GymName = gym.Name,
                                                 StartDate = userSubscr.StartDate,
                                                 EndDate = userSubscr.EndDate,
                                                 GroupTraining = subscr.GroupTraining,
                                                 DietMonitoring = subscr.DietMonitoring,
                                                 WorkAsTrainer = subscr.WorkAsTrainer,
                                                 Price = gymSubscr.Price
                                             })
                                             .ToList();

            return userSubscriptionsFullInfo;
        }



        public IEnumerable<UserSubscriptionEntityBase> GetValidSubscriptionsByUserForSpecificGym(string userId, int gymId)
        {
            var validSubscriptions = (from userSubscr in _context.UserSubscriptions
                                      join gymSubscr in _context.GymSubscriptions
                                          on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                      where userSubscr.UserId == userId
                                      where gymSubscr.GymId == gymId
                                      where userSubscr.StartDate <= DateTime.Today
                                      where userSubscr.EndDate >= DateTime.Today
                                      select new UserSubscriptionEntityBase()
                                      {
                                          Id = userSubscr.Id,
                                          UserId = userSubscr.UserId,
                                          GymSubscriptionId = userSubscr.GymSubscriptionId,
                                          StartDate = userSubscr.StartDate,
                                          EndDate = userSubscr.EndDate
                                      })
                                        .ToList();

            return validSubscriptions;
        }


        public IEnumerable<UserSubscriptionFullInfoBase> GetValidSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds)
        {
            var validUserSubscriptions = (from userSubscr in _context.UserSubscriptions
                                          join gymSubscr in _context.GymSubscriptions
                                          on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                          join subscr in _context.Subscriptions
                                          on gymSubscr.SubscriptionId equals subscr.Id
                                          join gym in _context.Gyms
                                          on gymSubscr.GymId equals gym.Id
                                          where userSubscr.UserId == userId
                                          where gymIds.Contains(gym.Id)
                                          where userSubscr.StartDate <= DateTime.Today
                                          where userSubscr.EndDate > DateTime.Today
                                          select new UserSubscriptionFullInfoBase()
                                          {
                                              Id = userSubscr.Id,
                                              UserId = userSubscr.UserId,
                                              GymName = gym.Name,
                                              StartDate = userSubscr.StartDate,
                                              EndDate = userSubscr.EndDate,
                                              GroupTraining = subscr.GroupTraining,
                                              DietMonitoring = subscr.DietMonitoring,
                                              WorkAsTrainer = subscr.WorkAsTrainer,
                                              Price = gymSubscr.Price
                                          })
                                         .ToList();

            return validUserSubscriptions;
        }


        public IEnumerable<UserSubscriptionFullInfoBase> GetExpiredSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds)
        {
            var expiredSubscriptions = (from userSubscr in _context.UserSubscriptions
                                        join gymSubscr in _context.GymSubscriptions
                                            on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                        join subscr in _context.Subscriptions
                                            on gymSubscr.SubscriptionId equals subscr.Id
                                        join gym in _context.Gyms
                                            on gymSubscr.GymId equals gym.Id
                                        where userSubscr.UserId == userId
                                        where gymIds.Contains(gym.Id)
                                        where userSubscr.StartDate < DateTime.Today
                                        where userSubscr.EndDate <= DateTime.Today
                                        select new UserSubscriptionFullInfoBase()
                                        {
                                            Id = userSubscr.Id,
                                            UserId = userSubscr.UserId,
                                            GymName = gym.Name,
                                            StartDate = userSubscr.StartDate,
                                            EndDate = userSubscr.EndDate,
                                            GroupTraining = subscr.GroupTraining,
                                            DietMonitoring = subscr.DietMonitoring,
                                            WorkAsTrainer = subscr.WorkAsTrainer,
                                            Price = gymSubscr.Price
                                        })
                                        .ToList();

            return expiredSubscriptions;
        }



        public IEnumerable<UserSubscriptionFullInfoBase> GetValidInTheFutureSubscriptionsByUserForGyms(string userId, IEnumerable<int> gymIds)
        {
            var validInTheFutureSubscriptions = (from userSubscr in _context.UserSubscriptions
                                                 join gymSubscr in _context.GymSubscriptions
                                                     on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                                 join subscr in _context.Subscriptions
                                                     on gymSubscr.SubscriptionId equals subscr.Id
                                                 join gym in _context.Gyms
                                                     on gymSubscr.GymId equals gym.Id
                                                 where userSubscr.UserId == userId
                                                 where gymIds.Contains(gym.Id)
                                                 where userSubscr.StartDate > DateTime.Today
                                                 where userSubscr.EndDate > DateTime.Today
                                                 select new UserSubscriptionFullInfoBase()
                                                 {
                                                     Id = userSubscr.Id,
                                                     UserId = userSubscr.UserId,
                                                     GymName = gym.Name,
                                                     StartDate = userSubscr.StartDate,
                                                     EndDate = userSubscr.EndDate,
                                                     GroupTraining = subscr.GroupTraining,
                                                     DietMonitoring = subscr.DietMonitoring,
                                                     WorkAsTrainer = subscr.WorkAsTrainer,
                                                     Price = gymSubscr.Price
                                                 })
                                                .ToList();

            return validInTheFutureSubscriptions;
        }



        public void UpdateUserSubscriptionDates(int subscriptionId, DateTime startDate, DateTime endDate)
        {
            var subscription = _context.UserSubscriptions.FirstOrDefault(x => x.Id == subscriptionId);
            if (subscription != null)
            {
                subscription.StartDate = startDate;
                subscription.EndDate = endDate;
                _context.SaveChanges();
            }
        }


        public IEnumerable<UserSubscriptionFullInfoBase> GetAllUsersByExpiringSubscriptions(int daysBeforeSubscrExpire)
        {
            try
            {
                var usersWithExpiringSubscriptions = (from userSubscr in _context.UserSubscriptions
                                                      join user in _context.Users
                                                          on userSubscr.UserId equals user.Id
                                                      join gymSubscr in _context.GymSubscriptions
                                                          on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                                      join gym in _context.Gyms
                                                          on gymSubscr.GymId equals gym.Id
                                                      where EF.Functions.DateDiffDay(userSubscr.StartDate, userSubscr.EndDate) > 1
                                                      where EF.Functions.DateDiffDay(DateTime.Today, userSubscr.EndDate) == daysBeforeSubscrExpire
                                                      select new UserSubscriptionFullInfoBase()
                                                      {
                                                          UserId = user.Id,
                                                          UserFirstName = user.FirstName,
                                                          UserLastName = user.LastName,
                                                          UserEmail = user.Email,
                                                          GymId = gym.Id,
                                                          GymName = gym.Name
                                                      }).ToList();

                return usersWithExpiringSubscriptions;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return null;
            }

        }





        //Events
        public IEnumerable<EventEntityBase> GetAllEvents()
        {
            var events = _context.Events.ToList();
            return events;

        }


        public IEnumerable<EventEntityBase> GetEventsByUser(string userId)
        {
            var userEvents = _context.Events
                .Where(x => x.UserId == userId)
                .ToList();

            return userEvents;
        }


        public IEnumerable<EventWithNamesBase> GetEventsByUserAndDate(string userId, DateTime dateTime)
        {
            string dateOnly = dateTime.ToString("yyyy-MM-dd");

            var eventsWithNames = (from events in _context.Events
                                   join user in _context.Users
                                   on events.UserId equals user.Id
                                   join userTr in _context.Users
                                   on events.TrainerId equals userTr.Id
                                   join trainer in _context.Trainers
                                   on events.TrainerId equals trainer.Id
                                   join license in _context.TrainerWorkLicenses
                                       on trainer.Id equals license.TrainerId
                                   join gym in _context.Gyms
                                   on license.GymId equals gym.Id
                                   join training in _context.Trainings
                                   on events.TrainingId equals training.Id
                                   where events.UserId == userId
                                   where events.Date.ToString() == dateOnly
                                   select new EventWithNamesBase()
                                   {
                                       Id = events.Id,
                                       Date = events.Date,
                                       StartTime = events.StartTime,
                                       EndTime = events.EndTime,
                                       TrainerId = events.TrainerId,
                                       TrainerFirstName = userTr.FirstName,
                                       TrainerLastName = userTr.LastName,
                                       GymId = gym.Id,
                                       GymName = gym.Name,
                                       UserId = events.UserId,
                                       TrainingId = events.TrainingId,
                                       TrainingName = training.Name,
                                       Status = events.Status
                                   })
                                   .OrderBy(x => x.StartTime)
                                   .ToList();

            return eventsWithNames;
        }



        public IEnumerable<EventWithNamesBase> GetEventsByTrainerAndDate(string trainerId, DateTime date)
        {
            string dateOnly = date.ToString("yyyy-MM-dd");

            var eventsWithNames = (from events in _context.Events
                                   join user in _context.Users
                                   on events.UserId equals user.Id
                                   join trainer in _context.Trainers
                                   on events.TrainerId equals trainer.Id
                                   join license in _context.TrainerWorkLicenses
                                   on events.TrainerId equals license.TrainerId
                                   join gym in _context.Gyms
                                   on license.GymId equals gym.Id
                                   join training in _context.Trainings
                                   on events.TrainingId equals training.Id
                                   where events.TrainerId == trainerId
                                   where events.Date.ToString() == dateOnly
                                   select new EventWithNamesBase()
                                   {
                                       Id = events.Id,
                                       Date = events.Date,
                                       StartTime = events.StartTime,
                                       EndTime = events.EndTime,
                                       UserId = events.UserId,
                                       UserFirstName = user.FirstName,
                                       UserLastName = user.LastName,
                                       TrainingId = events.TrainingId,
                                       TrainingName = training.Name,
                                       Status = events.Status
                                   })
                                    .OrderBy(x => x.StartTime)
                                    .ToList();

            return eventsWithNames;
        }




        // to show Events count for each date on the calendar for current User
        public IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId)
        {
            var allEventsByUser = _context.Events
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Date)
                .ToList();

            Dictionary<DateTime, int> dateEventCount = new Dictionary<DateTime, int>();

            foreach (var eventItem in allEventsByUser)
            {
                if (!dateEventCount.ContainsKey(eventItem.Date))
                {
                    int eventCount = allEventsByUser.Count(x => x.Date == eventItem.Date);
                    dateEventCount.Add(eventItem.Date, eventCount);
                }
            }

            return dateEventCount;
        }



        public IDictionary<DateTime, int> GetEventsCountForEachDateByTrainer(string trainerId)
        {
            var allEventsByTrainer = _context.Events
                .Where(x => x.TrainerId == trainerId)
                .OrderBy(x => x.Date)
                .ToList();

            Dictionary<DateTime, int> dateEventCount = new Dictionary<DateTime, int>();

            foreach (var eventItem in allEventsByTrainer)
            {
                if (!dateEventCount.ContainsKey(eventItem.Date))
                {
                    int eventCount = allEventsByTrainer.Count(x => x.Date == eventItem.Date);
                    dateEventCount.Add(eventItem.Date, eventCount);
                }
            }

            return dateEventCount;
        }


        public void ChangeEventStatus(int eventId)
        {
            var currentEvent = _context.Events.FirstOrDefault(x => x.Id == eventId);
            if (currentEvent != null)
            {
                if (currentEvent.Status == Common.EventStatusEnum.Open)
                {
                    currentEvent.Status = Common.EventStatusEnum.Confirmed;
                }
                else
                {
                    currentEvent.Status = Common.EventStatusEnum.Open;
                }
                _context.SaveChanges();
            }
        }


        public int GetActualEventsCountByTrainer(string trainerId)
        {
            var actualEventsCount = _context.Events
                .Where(x => x.TrainerId == trainerId)
                .Count(x => x.Date.Date >= DateTime.Now.Date);

            return actualEventsCount;
        }


        public IEnumerable<EventEntityBase> GetActualEventsByTrainer(string trainerId)
        {
            var actualEvents = _context.Events
                .Where(x => x.TrainerId == trainerId)
                .Where(x => x.Date.Date >= DateTime.Now.Date)
                .ToList();

            return actualEvents;
        }



        public bool AddEvent(EventEntityBase newEvent)
        {
            EventEntity newEventEntity = new EventEntity()
            {
                Date = newEvent.Date,
                StartTime = newEvent.StartTime,
                EndTime = newEvent.EndTime,
                TrainerId = newEvent.TrainerId,
                UserId = newEvent.UserId,
                TrainingId = newEvent.TrainingId,
                Status = newEvent.Status
            };

            _context.Events.Add(newEventEntity);
            int addedRowCount = _context.SaveChanges();
            return addedRowCount > 0;
        }



        //Chat

        public IEnumerable<ChatMessageEntityBase> GetAllMessagesByUser(string userId)
        {
            var messages = _context.ChatMessages
                .Where(x => x.ReceiverId == userId || x.SenderId == userId)
                .OrderBy(x => x.Date);
            return messages;
        }


        public IEnumerable<ChatMessageEntityBase> GetAllMessagesBetweenTwoUsers(string senderId, string receiverId)
        {
            var messages = _context.ChatMessages
                .Where(x => x.SenderId == senderId || x.ReceiverId == senderId)
                .Where(x => x.SenderId == receiverId || x.ReceiverId == receiverId)
                .OrderBy(x => x.Date)
                .Take(50);
            return messages;
        }


        public IEnumerable<string> GetAllContactsIdByUser(string userId)
        {
            var allContactsId = _context.ChatContacts
                .Where(x => x.UserId == userId)
                .Select(x => x.InterlocutorId)
                .ToList();
            return allContactsId;
        }


        public int AddMessage(ChatMessageEntityBase message)
        {
            ChatMessageEntity messageEntity = new ChatMessageEntity()
            {
                SenderId = message.SenderId,
                ReceiverId = message.ReceiverId,
                Message = message.Message,
                Date = message.Date
            };

            _context.ChatMessages.Add(messageEntity);
            _context.SaveChanges();
            return messageEntity.Id;
        }


        public ChatMessageEntityBase GetMessage(int messageId)
        {
            var message = _context.ChatMessages
                .FirstOrDefault(x => x.Id == messageId);

            return message;
        }


        public void AddContact(string userId, string interlocutorId)
        {
            ChatContactEntityBase newContact = new ChatContactEntity()
            {
                UserId = userId,
                InterlocutorId = interlocutorId
            };
            _context.Add(newContact);
            _context.SaveChanges();
        }


        //chart / diagrams

        public void DeleteNumberOfVisitorsPerHourChartData(int gymId)
        {
            var dataToDelete = _context.NumberOfVisitorsPerHour.Where(x => x.GymId == gymId).ToList();
            _context.RemoveRange(dataToDelete);
            _context.SaveChanges();
        }


        public void AddNumberOfVisitorsPerHourChartData(IEnumerable<NumberOfVisitorsPerHourEntityBase> chartData)
        {
            List<NumberOfVisitorsPerHourEntityBase> entities = new List<NumberOfVisitorsPerHourEntityBase>();
            foreach (var dataPerHour in chartData)
            {
                entities.Add(new NumberOfVisitorsPerHourEntity()
                {
                    GymId = dataPerHour.GymId,
                    DayOfWeekNumber = dataPerHour.DayOfWeekNumber,
                    Hour = dataPerHour.Hour,
                    NumberOfVisitors = dataPerHour.NumberOfVisitors
                });
            }

            _context.AddRange(entities);
            _context.SaveChanges();
        }


        public IEnumerable<NumberOfVisitorsPerHourEntityBase> GetNumOfVisitorsPerHourOnCertainDayByGym(int gymId, DayOfWeek day)
        {
            var numbersOfVisitorsPerHours = _context.NumberOfVisitorsPerHour
                .Where(x => x.GymId == gymId)
                .Where(x => x.DayOfWeekNumber == (int)day)
                .OrderBy(x => x.Hour)
                .ToList();

            return numbersOfVisitorsPerHours;
        }
    }
}
