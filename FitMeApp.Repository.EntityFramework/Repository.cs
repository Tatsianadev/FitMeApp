using FitMeApp.Repository.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities.JoinEntityBase;
using Microsoft.AspNetCore.Identity;

namespace FitMeApp.Repository.EntityFramework
{
    public class Repository : IRepository
    {
        ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
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
            GymEntity gym = _context.Gyms.Where(x => x.Id == id).First();
            return gym;
        }


        public GymEntityBase AddGym(GymEntityBase gym)
        {
            if (gym == null)
            {
                throw new NotImplementedException();
            }

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
            if (newGymData == null)
            {
                throw new NotImplementedException();
            }

            GymEntity gym = _context.Gyms.First(x => x.Id == id);

            gym.Name = newGymData.Name;
            gym.Address = newGymData.Address;
            gym.Phone = newGymData.Phone;

            _context.SaveChanges();
        }


        public void DeleteGym(int id)
        {
            GymEntity gym = _context.Gyms.First(x => x.Id == id);
            _context.Gyms.Remove(gym);
            _context.SaveChanges();
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
                .First();
            return gymWorkHoursId;
        }


        //Trainers//

        public IEnumerable<TrainerEntityBase> GetAllTrainers()
        {
            var trainers = _context.Trainers.ToList();
            return trainers;
        }


        public IEnumerable<TrainerWithGymAndTrainingsBase> GetAllTrainersByStatus(TrainerApproveStatusEnum status)
        {
            var trainers = (from trainer in _context.Trainers
                            join user in _context.Users
                            on trainer.Id equals user.Id
                            where trainer.Status == status
                            select new TrainerWithGymAndTrainingsBase()
                            {
                                Id = trainer.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                Status = trainer.Status
                            }).ToList();

            return trainers;
        }


        public TrainerEntityBase GetTrainer(string id)
        {
            var trainer = _context.Trainers
                .Where(x => x.Id == id)
                .First();
            return trainer;
        }


        public bool AddTrainer(TrainerEntityBase trainer)
        {
            if (trainer == null)
            {
                throw new ArgumentNullException(nameof(trainer));
            }

            _context.Trainers.Add(new TrainerEntity()
            {
                Id = trainer.Id,
                Specialization = trainer.Specialization,
                GymId = trainer.GymId,
                Status = trainer.Status
            });

            int addedRowCount = _context.SaveChanges();
            return addedRowCount > 0 ? true : false;
        }


        public void UpdateTrainer(TrainerEntityBase newTrainerData)
        {

            if (newTrainerData == null)
            {
                throw new ArgumentNullException(nameof(newTrainerData));
            }

            var trainer = _context.Trainers
                .Where(x => x.Id == newTrainerData.Id)
                .First();

            trainer.Specialization = newTrainerData.Specialization;
            trainer.Status = newTrainerData.Status;
            trainer.GymId = newTrainerData.GymId;

            _context.SaveChanges();
        }


        public void DeleteTrainer(string id)
        {
            var trainer = _context.Trainers
                .Where(x => x.Id == id)
                .First();

            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
            }
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
                                    .First();

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



        //Edit Trainer WorkHours methods
        public bool AddTrainerWorkHours(TrainerWorkHoursEntityBase workHoursBase)
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

                int addedRowsCount = _context.SaveChanges();
                return addedRowsCount > 0 ? true : false;
            }
            else
            {
                return false;
            }
        }


        public void DeleteTrainerWorkHours(int workHoursId)
        {
            _context.Remove(_context.TrainerWorkHours.Where(x => x.Id == workHoursId).First());
            _context.SaveChanges();
        }


        public void UpdateTrainerWorkHours(TrainerWorkHoursEntityBase newTrainerWorkHours)
        {
            TrainerWorkHoursEntity workHoursEntity = _context.TrainerWorkHours
                .Where(x => x.Id == newTrainerWorkHours.Id)
                .First();

            workHoursEntity.StartTime = newTrainerWorkHours.StartTime;
            workHoursEntity.EndTime = newTrainerWorkHours.EndTime;
            _context.SaveChanges();
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
            var training = _context.Trainings
                .Where(x => x.Id == id)
                .First();

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

            var groupClass = _context.Trainings
                .Where(x => x.Id == id)
                .First();

            groupClass.Name = newGroupClassData.Name;
            groupClass.Description = newGroupClassData.Description;

            _context.SaveChanges();
        }


        public void DeleteTraining(int id)
        {
            var groupClass = _context.Trainings
                .Where(x => x.Id == id)
                .First();

            _context.Trainings.Remove(groupClass);
            _context.SaveChanges();
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
                .Where(x => x.TrainingId == trainingToDeleteId)
                .First();

            _context.TrainingTrainer.Remove(trainingTrainersConnection);
            _context.SaveChanges();
        }


        public bool AddTrainingTrainerConnection(string trainerId, int trainingToAddId)
        {
            _context.TrainingTrainer.Add(new TrainingTrainerEntity()
            {
                TrainerId = trainerId,
                TrainingId = trainingToAddId
            });

            int addedCount = _context.SaveChanges();
            return addedCount > 0 ? true : false;
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
                                          join trainer in _context.Trainers
                                          on gymDb.Id equals trainer.GymId
                                          join user in _context.Users
                                          on trainer.Id equals user.Id
                                          join trainingTrainer in _context.TrainingTrainer
                                          on trainer.Id equals trainingTrainer.TrainerId
                                          join training in _context.Trainings
                                          on trainingTrainer.TrainingId equals training.Id
                                          join image in _context.GymImages
                                              on gymDb.Id equals image.GymId
                                          where gymDb.Id == gymId
                                          where trainer.Status == TrainerApproveStatusEnum.approved
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
                                              TrainingDescription = training.Description,
                                              TrainerStatus = trainer.Status
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
                        Trainings = trainings,
                        Status = item.TrainerStatus
                    });
                    addedTrainersId.Add(item.TrainerId);
                }
                else
                {
                    var currentTrainer = trainers.Where(x => x.Id == item.TrainerId).First();
                    var currentTrainerTrainings = currentTrainer.Trainings.ToList();
                    currentTrainerTrainings.Add(training);
                    trainers.Where(x => x.Id == item.TrainerId).First().Trainings = currentTrainerTrainings;
                }
            }

            gym.Trainers = trainers;
            return gym;
        }



        public List<TrainerWithGymAndTrainingsBase> GetAllTrainersWithGymAndTrainings()
        {
            var allTrainersGymTrainingsJoin = (from trainer in _context.Trainers
                                               join user in _context.Users
                                               on trainer.Id equals user.Id
                                               join gym in _context.Gyms
                                               on trainer.GymId equals gym.Id
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
                                                   Status = trainer.Status,
                                                   GymId = gym.Id,
                                                   GymName = gym.Name,
                                                   GymAddress = gym.Address,
                                                   TrainingId = training.Id,
                                                   TrainingName = training.Name
                                               }).OrderBy(x => x.TrainerId).ToList();


            List<TrainerWithGymAndTrainingsBase> trainersWithGymAndTrainings = ConvertJoinResultToTrainerWithGymAndTrainingsBase(allTrainersGymTrainingsJoin);

            return trainersWithGymAndTrainings;
        }


        private List<TrainerWithGymAndTrainingsBase> ConvertJoinResultToTrainerWithGymAndTrainingsBase(List<TrainerWithGymAndTrainingsJoin> trainersGymsTrainingsJoins)
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
                        Status = item.Status,
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
                                          join gym in _context.Gyms
                                          on trainer.GymId equals gym.Id
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
                                              Status = trainer.Status,
                                              GymId = trainer.GymId,
                                              GymName = gym.Name,
                                              GymAddress = gym.Address,
                                              TrainingId = training.Id,
                                              TrainingName = training.Name
                                          }).ToList();

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
                Status = trainerEntity.Status,
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


        //Filters

        public IEnumerable<GymEntityBase> GetGymsByTrainings(List<int> trainingsId)
        {
            var gymsByTrainings = (from trainingTrainer in _context.TrainingTrainer
                                   join trainer in _context.Trainers
                                   on trainingTrainer.TrainerId equals trainer.Id
                                   join gymDb in _context.Gyms
                                   on trainer.GymId equals gymDb.Id
                                   where trainingsId.Contains(trainingTrainer.TrainingId)
                                   select new GymEntityBase()
                                   {
                                       Id = gymDb.Id,
                                       Name = gymDb.Name,
                                       Address = gymDb.Address,
                                       Phone = gymDb.Phone
                                   }).ToList().Distinct();

            return gymsByTrainings;
        }


        public IEnumerable<SubscriptionPriceBase> GetSubscriptionsForVisitorsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring)
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


        public IEnumerable<TrainerWithGymAndTrainingsBase> GetTrainersWithGymAndTrainingsByFilter(List<string> selectedGenders, List<string> selectedSpecializations)
        {
            var trainersGymTrainingsByFilterJoin = (from trainer in _context.Trainers
                                                    join user in _context.Users
                                                    on trainer.Id equals user.Id
                                                    join gym in _context.Gyms
                                                    on trainer.GymId equals gym.Id
                                                    join trainingTrainer in _context.TrainingTrainer
                                                    on trainer.Id equals trainingTrainer.TrainerId
                                                    join training in _context.Trainings
                                                    on trainingTrainer.TrainingId equals training.Id
                                                    where selectedGenders.Contains(user.Gender)
                                                    where selectedSpecializations.Contains(trainer.Specialization)
                                                    where trainer.Status == TrainerApproveStatusEnum.approved
                                                    select new TrainerWithGymAndTrainingsJoin()
                                                    {
                                                        TrainerId = trainer.Id,
                                                        FirstName = user.FirstName,
                                                        LastName = user.LastName,
                                                        Gender = user.Gender,
                                                        AvatarPath = user.AvatarPath,
                                                        Specialization = trainer.Specialization,
                                                        Status = trainer.Status,
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



        public List<int> GetAllSubscriptionPeriods()
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
                .Where(x => x.Id == subscriptionId)
                .First()
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


        //UserSubscriptions
        public bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate)
        {
            int gymSubscriptionId = _context.GymSubscriptions
                .Where(x => x.GymId == gymId && x.SubscriptionId == subscriptionId)
                .First()
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

            int addedEntry = _context.SaveChanges();
            return addedEntry > 0 ? true : false;
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


        public IEnumerable<UserSubscriptionFullInfoBase> GetValidSubscriptionsByUserForGyms(string userId, List<int> gymIds)
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


        public IEnumerable<UserSubscriptionFullInfoBase> GetExpiredSubscriptionsByUserForGyms(string userId, List<int> gymIds)
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



        public IEnumerable<UserSubscriptionFullInfoBase> GetValidInTheFutureSubscriptionsByUserForGyms(string userId, List<int> gymIds)
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
                                   join gym in _context.Gyms
                                   on trainer.GymId equals gym.Id
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
                                       GymId = trainer.GymId,
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
                                   join gym in _context.Gyms
                                   on trainer.GymId equals gym.Id
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
                    int eventCount = allEventsByUser.Where(x => x.Date == eventItem.Date).Count();
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
                    int eventCount = allEventsByTrainer.Where(x => x.Date == eventItem.Date).Count();
                    dateEventCount.Add(eventItem.Date, eventCount);
                }
            }

            return dateEventCount;
        }


        public bool ChangeEventStatus(int eventId)
        {
            var currentEvent = _context.Events.Where(x => x.Id == eventId).First();

            if (currentEvent.Status == Common.EventStatusEnum.Open)
            {
                currentEvent.Status = Common.EventStatusEnum.Confirmed;
            }
            else
            {
                currentEvent.Status = Common.EventStatusEnum.Open;
            }
            var changedEntry = _context.SaveChanges();
            return changedEntry > 0 ? true : false;
        }


        public int GetActualEventsCountByTrainer(string trainerId)
        {
            var actualEventsCount = _context.Events
                .Where(x => x.TrainerId == trainerId)
                .Where(x => x.Date.Date >= DateTime.Now.Date)
                .ToList()
                .Count();

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
            return addedRowCount > 0 ? true : false;
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
                .Where(x => x.Id == messageId)
                .First();
            return message;
        }


        public bool AddContact(string userId, string interlocutorId)
        {
            ChatContactEntityBase newContact = new ChatContactEntity()
            {
                UserId = userId,
                InterlocutorId = interlocutorId
            };
            _context.Add(newContact);
            int addedContactCount = _context.SaveChanges();
            return addedContactCount > 0 ? true : false;
        }




    }
}
