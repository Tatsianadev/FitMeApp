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

        public bool UpdateGym(int id, GymEntityBase newGymData)
        {
            if (newGymData == null)
            {
                throw new NotImplementedException();
            }

            GymEntity gym = _context.Gyms.First(x => x.Id == id);

            gym.Name = newGymData.Name;
            gym.Address = newGymData.Address;
            gym.Phone = newGymData.Phone;

            var result = _context.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteGym(int id)
        {
            GymEntity gym = _context.Gyms.First(x => x.Id == id);
            _context.Gyms.Remove(gym);
            var result = _context.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<GymWorkHoursEntityBase> GetWorkHoursByGym(int gymId)
        {
            var gymWorkHours = _context.GymWorkHours.Where(x => x.GymId == gymId).OrderBy(x => x.DayOfWeekNumber).ToList();
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
            var trainersUsersJoin = (from trainer in _context.Trainers
                                     join user in _context.Users
                                     on trainer.Id equals user.Id
                                     where trainer.Status == status
                                     select new
                                     {
                                         Id = trainer.Id,
                                         FirstName = user.FirstName,
                                         LastName = user.LastName,
                                         Status = trainer.Status
                                     }).ToList();

            List<TrainerWithGymAndTrainingsBase> trainers = new List<TrainerWithGymAndTrainingsBase>();
            foreach (var trainerUserJoin in trainersUsersJoin)
            {
                trainers.Add(new TrainerWithGymAndTrainingsBase()
                {
                    Id = trainerUserJoin.Id,
                    FirstName = trainerUserJoin.FirstName,
                    LastName = trainerUserJoin.LastName,
                    Status = trainerUserJoin.Status
                });
            }

            return trainers;
        }

        public TrainerEntityBase GetTrainer(string id)
        {
            var trainer = _context.Trainers.Where(x => x.Id == id).First();
            return trainer;
        }

        public bool AddTrainer(TrainerEntityBase trainer)
        {
            if (trainer == null)
            {
                throw new NotImplementedException();
            }

            _context.Trainers.Add(new TrainerEntity()
            {
                Id = trainer.Id,
                Specialization = trainer.Specialization,
                GymId = trainer.GymId,
                Status = trainer.Status
            });

            int addedRowCount = _context.SaveChanges();
            if (addedRowCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public void UpdateTrainer(TrainerEntityBase newTrainerData)
        {

            if (newTrainerData == null)
            {
                throw new NotImplementedException();
            }

            var trainer = _context.Trainers.Where(x => x.Id == newTrainerData.Id).First();
            trainer.Specialization = newTrainerData.Specialization;
            trainer.Status = newTrainerData.Status;
            trainer.GymId = newTrainerData.GymId;

            _context.SaveChanges();

        }


        public void DeleteTrainer(string id)
        {
            var trainer = _context.Trainers.Where(x => x.Id == id).First();
            if (trainer != null)
            {
                _context.Trainers.Remove(trainer);
                _context.SaveChanges();
            }
        }


        public IEnumerable<TrainerWorkHoursWithDayBase> GetWorkHoursByTrainer(string trainerId)
        {
            var trainerWorkHoursGymWorkHoursJoin = (from trainerWorkHours in _context.TrainerWorkHours
                                                    join gymWorkHours in _context.GymWorkHours
                                                    on trainerWorkHours.GymWorkHoursId equals gymWorkHours.Id
                                                    where trainerWorkHours.TrainerId == trainerId
                                                    select new
                                                    {
                                                        Id = trainerWorkHours.Id,
                                                        TrainerId = trainerWorkHours.TrainerId,
                                                        StartTime = trainerWorkHours.StartTime,
                                                        EndTime = trainerWorkHours.EndTime,
                                                        GymWorkHoursId = trainerWorkHours.GymWorkHoursId,
                                                        DayName = gymWorkHours.DayOfWeekNumber
                                                    }).OrderBy(x => x.DayName).ToList();

            List<TrainerWorkHoursWithDayBase> trainerWorkHoursWithDays = new List<TrainerWorkHoursWithDayBase>();
            foreach (var item in trainerWorkHoursGymWorkHoursJoin)
            {
                trainerWorkHoursWithDays.Add(new TrainerWorkHoursWithDayBase()
                {
                    Id = item.Id,
                    TrainerId = item.TrainerId,
                    StartTime = item.StartTime,
                    EndTime = item.EndTime,
                    GymWorkHoursId = item.GymWorkHoursId,
                    DayName = item.DayName
                });
            }
            return trainerWorkHoursWithDays;

        }

        public IEnumerable<string> GetAllClientsIdByTrainer(string trainerId)
        {
            List<string> clientsId = _context.UserSubscriptions
                .Where(x => x.TrainerId == trainerId)
                .Select(x => x.UserId)
                .ToList();

            return clientsId;
        }


        public void DeleteTrainerWorkHoursByTrainer(string trainerId)
        {
            var trainerWorkHours = _context.TrainerWorkHours.Where(x => x.TrainerId == trainerId).ToList();
            if (trainerWorkHours.Count > 0)
            {
                _context.TrainerWorkHours.RemoveRange(trainerWorkHours);
                _context.SaveChanges();
            }
        }


        public IEnumerable<int> GetAvailableToApplyTrainingTimingByTrainer(string trainerId, DateTime date)
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

            List<int> workTimingScale = new List<int>();
            for (int timeInMinutes = trainerWorkHoursJoin.StartTime; timeInMinutes < trainerWorkHoursJoin.EndTime; timeInMinutes = (timeInMinutes + 30))
            {
                workTimingScale.Add(timeInMinutes);
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

            List<int> availableTime = workTimingScale
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
                if (addedRowsCount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
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

        public bool UpdateTrainerWorkHours(TrainerWorkHoursEntityBase newTrainerWorkHours)
        {
            TrainerWorkHoursEntity workHoursEntity = _context.TrainerWorkHours
                .Where(x => x.Id == newTrainerWorkHours.Id)
                .First();

            workHoursEntity.StartTime = newTrainerWorkHours.StartTime;
            workHoursEntity.EndTime = newTrainerWorkHours.EndTime;
            _context.SaveChanges();

            return true;

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
            var training = _context.Trainings.Where(x => x.Id == id).First();
            return training;
        }


        public TrainingEntityBase AddTraining(TrainingEntityBase item)
        {
            if (item == null)
            {
                throw new NotImplementedException();
            }

            _context.Add(new TrainingEntity()
            {
                Name = item.Name,
                Description = item.Description
            });
            _context.SaveChanges();
            return item;
        }


        public bool UpdateTraining(int id, TrainingEntityBase newGroupClassData)
        {
            if (newGroupClassData == null)
            {
                throw new NotImplementedException();
            }

            var groupClass = _context.Trainings.Where(x => x.Id == id).First();
            groupClass.Name = newGroupClassData.Name;
            groupClass.Description = newGroupClassData.Description;

            var result = _context.SaveChanges();
            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void DeleteTraining(int id)
        {
            var groupClass = _context.Trainings.Where(x => x.Id == id).First();
            _context.Trainings.Remove(groupClass);
            _context.SaveChanges();
        }

        //Trainer-Trainings

        public void DeleteTrainingTrainerConnection(string trainerId, int trainingToDeleteId)
        {
            var trainingTrainersConnection = _context.TrainingTrainer.Where(x => x.TrainerId == trainerId).Where(x => x.TrainingId == trainingToDeleteId).First();
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
            if (addedCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public void DeleteAllTrainingTrainerConnectionsByTrainer(string trainerId)
        {
            var allTrainingsByTrainer = _context.TrainingTrainer.Where(x => x.TrainerId == trainerId).ToList();
            if (allTrainingsByTrainer.Count > 0)
            {
                _context.TrainingTrainer.RemoveRange(allTrainingsByTrainer);
                _context.SaveChanges();
            }
        }

        //Gym - Trainer - Trainings connection

        public GymWithTrainersAndTrainings GetGymWithTrainersAndTrainings(int gymId)
        {
            try
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
                                              where gymDb.Id == gymId
                                              where trainer.Status == TrainerApproveStatusEnum.approved
                                              select new
                                              {
                                                  GymId = gymDb.Id,
                                                  GymName = gymDb.Name,
                                                  GymAddress = gymDb.Address,
                                                  GymPhone = gymDb.Phone,
                                                  TrainerId = trainer.Id,
                                                  TrainerFirstName = user.FirstName,
                                                  TrainerLastName = user.LastName,
                                                  TrainerGender = user.Gender,
                                                  TrainerPicture = user.Avatar,
                                                  TrainerSpecialization = trainer.Specialization,
                                                  TrainingId = training.Id,
                                                  TrainingName = training.Name,
                                                  TrainingDescription = training.Description,
                                                  TrainerStatus = trainer.Status
                                              }).ToList();

                var trainers = new List<TrainerWithGymAndTrainingsBase>();
                List<string> addedTrainersId = new List<string>();

                GymWithTrainersAndTrainings gym = new GymWithTrainersAndTrainings();
                var gymInfo = gymTrainerTrainingJoin.First();
                gym.Id = gymInfo.GymId;
                gym.Name = gymInfo.GymName;
                gym.Phone = gymInfo.GymPhone;
                gym.Address = gymInfo.GymAddress;

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
                            Picture = item.TrainerPicture,
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
            catch (Exception ex)
            {

                throw ex;
            }
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
                                                   Picture = user.Avatar,
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
                        Picture = item.Picture,
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
                                              Picture = user.Avatar,
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
                Picture = trainerEntity.Picture,
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


        public bool UpdateTrainerWithGymAndTrainings(TrainerWithGymAndTrainingsBase newTrainerInfo)  // return void?
        {
            TrainerEntityBase newTrainerEntityBase = new TrainerEntityBase()
            {
                Id = newTrainerInfo.Id,
                GymId = newTrainerInfo.Gym.Id,
                Specialization = newTrainerInfo.Specialization,
                Status = newTrainerInfo.Status
            };

            UpdateTrainer(newTrainerEntityBase);

            var previousTrainingsId = _context.TrainingTrainer.Where(x => x.TrainerId == newTrainerEntityBase.Id).Select(x => x.TrainingId).ToList();
            var newTrainingsId = newTrainerInfo.Trainings.Select(x => x.Id).ToList();

            var trainingsIdToDelete = previousTrainingsId.Except(newTrainingsId);
            var trainingsIdToAdd = newTrainingsId.Except(previousTrainingsId);

            foreach (var trainingId in trainingsIdToDelete)
            {
                DeleteTrainingTrainerConnection(newTrainerEntityBase.Id, trainingId);
            }

            foreach (var trainingId in trainingsIdToAdd)
            {
                AddTrainingTrainerConnection(newTrainerEntityBase.Id, trainingId);
            }
            return true;
        }




        public TrainingWithTrainerAndGymBase GetTrainingWithTrainerAndGym(int groupClassId)
        {


            return null;
        }






        //Filters

        public IEnumerable<GymEntityBase> GetGymsByTrainings(List<int> trainingsId)
        {
            var gymsByTrainingIdJoin = (from trainingTrainer in _context.TrainingTrainer
                                        join trainer in _context.Trainers
                                        on trainingTrainer.TrainerId equals trainer.Id
                                        join gymDb in _context.Gyms
                                        on trainer.GymId equals gymDb.Id
                                        where trainingsId.Contains(trainingTrainer.TrainingId)
                                        select new
                                        {
                                            GymId = gymDb.Id,
                                            GymName = gymDb.Name,
                                            GymAddress = gymDb.Address,
                                            GymPhone = gymDb.Phone
                                        }).ToList().Distinct();

            List<GymEntityBase> gymsByTrainings = new List<GymEntityBase>();
            foreach (var gym in gymsByTrainingIdJoin)
            {
                gymsByTrainings.Add(new GymEntityBase()
                {
                    Id = gym.GymId,
                    Name = gym.GymName,
                    Address = gym.GymAddress,
                    Phone = gym.GymPhone
                });
            }

            return gymsByTrainings;
        }


        public IEnumerable<SubscriptionPriceBase> GetSubscriptionsForVisitorsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring)
        {
            var subscriptionsPriceByGymByFiltersJoin = (from gymSubscription in _context.GymSubscriptions
                                                        join subscription in _context.Subscriptions
                                                        on gymSubscription.SubscriptionId equals subscription.Id
                                                        where gymSubscription.GymId == gymId
                                                        where periods.Contains(subscription.ValidDays)
                                                        where subscription.GroupTraining == groupTraining
                                                        where subscription.DietMonitoring == dietMonitoring
                                                        where subscription.WorkAsTrainer == false
                                                        select new
                                                        {
                                                            SubscriptionId = subscription.Id,
                                                            GymId = gymSubscription.GymId,
                                                            ValidDays = subscription.ValidDays,
                                                            GroupTraining = subscription.GroupTraining,
                                                            DietMonitoring = subscription.DietMonitoring,
                                                            Price = gymSubscription.Price
                                                        });

            List<SubscriptionPriceBase> subscriptions = new List<SubscriptionPriceBase>();
            foreach (var subscription in subscriptionsPriceByGymByFiltersJoin)
            {
                subscriptions.Add(new SubscriptionPriceBase()
                {
                    Id = subscription.SubscriptionId,
                    GymId = subscription.GymId,
                    ValidDays = subscription.ValidDays,
                    GroupTraining = subscription.GroupTraining,
                    DietMonitoring = subscription.DietMonitoring,
                    Price = subscription.Price
                });
            }
            return subscriptions;
        }


        public IEnumerable<TrainerWithGymAndTrainingsBase> GetTrainersWithGymAndTrainengsByFilter(List<string> selectedGenders, List<string> selectedSpecializations)
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
                                                        Picture = user.Avatar,
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
            var subscriptionsPriceByGymJoin = (from gymSubscription in _context.GymSubscriptions
                                               join subscription in _context.Subscriptions
                                               on gymSubscription.SubscriptionId equals subscription.Id
                                               where gymSubscription.GymId == gymId
                                               where subscription.WorkAsTrainer == false
                                               select new
                                               {
                                                   SubscriptionId = subscription.Id,
                                                   GymId = gymSubscription.GymId,
                                                   ValidDays = subscription.ValidDays,
                                                   GroupTraining = subscription.GroupTraining,
                                                   DietMonitoring = subscription.DietMonitoring,
                                                   Price = gymSubscription.Price
                                               });
            List<SubscriptionPriceBase> subscriptionsByGym = new List<SubscriptionPriceBase>();
            foreach (var subscription in subscriptionsPriceByGymJoin)
            {
                subscriptionsByGym.Add(new SubscriptionPriceBase()
                {
                    Id = subscription.SubscriptionId,
                    GymId = subscription.GymId,
                    ValidDays = subscription.ValidDays,
                    GroupTraining = subscription.GroupTraining,
                    DietMonitoring = subscription.DietMonitoring,
                    Price = subscription.Price
                });
            }

            return subscriptionsByGym;
        }


        public IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsForTrainersByGym(int gymId)
        {
            var subscriptionsPriceByGymJoin = (from gymSubscription in _context.GymSubscriptions
                                               join subscription in _context.Subscriptions
                                                   on gymSubscription.SubscriptionId equals subscription.Id
                                               where gymSubscription.GymId == gymId
                                               where subscription.WorkAsTrainer == true
                                               select new
                                               {
                                                   SubscriptionId = subscription.Id,
                                                   GymId = gymSubscription.GymId,
                                                   ValidDays = subscription.ValidDays,
                                                   GroupTraining = subscription.GroupTraining,
                                                   DietMonitoring = subscription.DietMonitoring,
                                                   WorkAsTRainer = subscription.WorkAsTrainer,
                                                   Price = gymSubscription.Price
                                               });

            List<SubscriptionPriceBase> subscriptionsByGym = new List<SubscriptionPriceBase>();
            foreach (var subscription in subscriptionsPriceByGymJoin)
            {
                subscriptionsByGym.Add(new SubscriptionPriceBase()
                {
                    Id = subscription.SubscriptionId,
                    GymId = subscription.GymId,
                    ValidDays = subscription.ValidDays,
                    GroupTraining = subscription.GroupTraining,
                    DietMonitoring = subscription.DietMonitoring,
                    WorkAsTrainer = subscription.WorkAsTRainer,
                    Price = subscription.Price
                });
            }

            return subscriptionsByGym;
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
            int subscriptionPeriod = _context.Subscriptions.Where(x => x.Id == subscriptionId).First().ValidDays;
            return subscriptionPeriod;
        }


        public SubscriptionPriceBase GetSubscriptionWithPriceByGym(int subscriptionId, int gymId)
        {
            var subscriptionGymJoin = (from gymSubscription in _context.GymSubscriptions
                                       join subscription in _context.Subscriptions
                                       on gymSubscription.SubscriptionId equals subscription.Id
                                       where gymSubscription.GymId == gymId
                                       where subscription.Id == subscriptionId
                                       select new
                                       {
                                           SubscriptionId = subscription.Id,
                                           GymId = gymSubscription.GymId,
                                           ValidDays = subscription.ValidDays,
                                           GroupTraining = subscription.GroupTraining,
                                           DietMonitoring = subscription.DietMonitoring,
                                           Price = gymSubscription.Price
                                       }).First();

            SubscriptionPriceBase subscriptionWithPrice = new SubscriptionPriceBase()
            {
                Id = subscriptionGymJoin.SubscriptionId,
                GymId = subscriptionGymJoin.GymId,
                ValidDays = subscriptionGymJoin.ValidDays,
                GroupTraining = subscriptionGymJoin.GroupTraining,
                DietMonitoring = subscriptionGymJoin.DietMonitoring,
                Price = subscriptionGymJoin.Price
            };
            return subscriptionWithPrice;
        }


        //UserSubscriptions
        public bool AddUserSubscription(string userId, int gymId, int subscriptionId, DateTime startDate)
        {
            int gymSubscriptionId = _context.GymSubscriptions.Where(x => x.GymId == gymId && x.SubscriptionId == subscriptionId).First().Id;
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
            bool result = addedEntry > 0 ? true : false;

            return result;

        }


        public int GetActualSubscriptionsCountByTrainer(string trainerId)
        {
            var actualSubscriptionsCount = _context.UserSubscriptions
                .Where(x => x.TrainerId == trainerId)
                .Where(x => x.EndDate.Date >= DateTime.Now.Date)
                .ToList().Count();
            return actualSubscriptionsCount;
        }

        public IEnumerable<UserSubscriptionWithIncludedOptionsBase> GetUserSubscriptionsFullInfo(string userId)
        {
            var userSubscriptionsJoin = (from userSubscr in _context.UserSubscriptions
                                         join gymSubscr in _context.GymSubscriptions
                                         on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                         join subscr in _context.Subscriptions
                                         on gymSubscr.SubscriptionId equals subscr.Id
                                         where userSubscr.UserId == userId
                                         select new
                                         {
                                             Id = userSubscr.Id,
                                             UserId = userSubscr.UserId,
                                             GymSubscriptionId = userSubscr.GymSubscriptionId,
                                             TrainerId = userSubscr.TrainerId,
                                             StartDate = userSubscr.StartDate,
                                             EndDate = userSubscr.EndDate,
                                             GroupTraining = subscr.GroupTraining,
                                             DietMonitoring = subscr.DietMonitoring
                                         }).ToList();

            List<UserSubscriptionWithIncludedOptionsBase> userSubscriptions = new List<UserSubscriptionWithIncludedOptionsBase>();
            foreach (var joinItem in userSubscriptionsJoin)
            {
                userSubscriptions.Add(new UserSubscriptionWithIncludedOptionsBase()
                {
                    Id = joinItem.Id,
                    UserId = joinItem.UserId,
                    GymSubscriptionId = joinItem.GymSubscriptionId,
                    TrainerId = joinItem.TrainerId,
                    StartDate = joinItem.StartDate,
                    EndDate = joinItem.EndDate,
                    GroupTraining = joinItem.GroupTraining,
                    DietMonitoring = joinItem.DietMonitoring
                });
            }

            return userSubscriptions;
        }



        public IEnumerable<UserSubscriptionEntityBase> GetActualSubscriptionsByUser(string userId)
        {
            var subscriptions = _context.UserSubscriptions
                .Where(x => x.UserId == userId)
                .Where(x => x.EndDate > DateTime.Today)
                .ToList();

            return subscriptions;
        }


        public IEnumerable<UserSubscriptionEntityBase> GetActualSubscriptionsByUserForSpecificGym(string userId, int gymId)
        {
            var userSubscriptionsJoin = (from userSubscr in _context.UserSubscriptions
                                         join gymSubscr in _context.GymSubscriptions
                                             on userSubscr.GymSubscriptionId equals gymSubscr.Id
                                         where userSubscr.UserId == userId
                                         where gymSubscr.GymId == gymId
                                         where userSubscr.EndDate > DateTime.Now
                                         select new
                                         {
                                             Id = userSubscr.Id,
                                             UserId = userSubscr.UserId,
                                             GymSubscriptionId = userSubscr.GymSubscriptionId,
                                             TrainerId = userSubscr.TrainerId,
                                             StartDate = userSubscr.StartDate,
                                             EndDate = userSubscr.EndDate
                                         }).ToList();

            List<UserSubscriptionEntityBase> subscriptions = new List<UserSubscriptionEntityBase>();
            foreach (var joinItem in userSubscriptionsJoin)
            {
                subscriptions.Add(new UserSubscriptionEntity()
                {
                    Id = joinItem.Id,
                    UserId = joinItem.UserId,
                    GymSubscriptionId = joinItem.GymSubscriptionId,
                    TrainerId = joinItem.TrainerId,
                    StartDate = joinItem.StartDate,
                    EndDate = joinItem.EndDate
                });
            }

            return subscriptions;
        }


        //Events
        public IEnumerable<EventEntityBase> GetAllEvents()
        {
            var events = _context.Events.ToList();
            return events;

        }


        public IEnumerable<EventEntityBase> GetEventsByUser(string userId)
        {
            var userEvents = _context.Events.Where(x => x.UserId == userId).ToList();
            return userEvents;
        }


        public IEnumerable<EventWithNamesBase> GetEventsByUserAndDate(string userId, DateTime dateTime)
        {
            string dateOnly = dateTime.ToString("yyyy-MM-dd");

            var eventTrainerTrainingUserGymJoin = (from events in _context.Events
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
                                                   select new
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
                                                       //UserName = user.UserName,
                                                       TrainingId = events.TrainingId,
                                                       TrainingName = training.Name,
                                                       Status = events.Status
                                                   })
                                                       .OrderBy(x => x.StartTime).ToList();

            List<EventWithNamesBase> eventsEntityBases = new List<EventWithNamesBase>();
            foreach (var entity in eventTrainerTrainingUserGymJoin)
            {
                eventsEntityBases.Add(new EventWithNamesBase()
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    StartTime = entity.StartTime,
                    EndTime = entity.EndTime,
                    TrainerId = entity.TrainerId,
                    TrainerFirstName = entity.TrainerFirstName,
                    TrainerLastName = entity.TrainerLastName,
                    GymId = entity.GymId,
                    GymName = entity.GymName,
                    UserId = entity.UserId,
                    //UserName = entity.UserName,
                    TrainingId = entity.TrainingId,
                    TrainingName = entity.TrainingName,
                    Status = entity.Status
                });
            }

            return eventsEntityBases;
        }



        public IEnumerable<EventWithNamesBase> GetEventsByTrainerAndDate(string trainerId, DateTime date)
        {
            string dateOnly = date.ToString("yyyy-MM-dd");

            var eventTrainerTrainingUserGymJoin = (from events in _context.Events
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
                                                   select new
                                                   {
                                                       Id = events.Id,
                                                       Date = events.Date,
                                                       StartTime = events.StartTime,
                                                       EndTime = events.EndTime,
                                                       UserId = events.UserId,
                                                       //UserName = user.UserName,
                                                       UserFirstName = user.FirstName,
                                                       UserLastName = user.LastName,
                                                       TrainingId = events.TrainingId,
                                                       TrainingName = training.Name,
                                                       Status = events.Status
                                                   }).OrderBy(x => x.StartTime).ToList();

            List<EventWithNamesBase> eventsEntityBases = new List<EventWithNamesBase>();
            foreach (var entity in eventTrainerTrainingUserGymJoin)
            {
                eventsEntityBases.Add(new EventWithNamesBase()
                {
                    Id = entity.Id,
                    Date = entity.Date,
                    StartTime = entity.StartTime,
                    EndTime = entity.EndTime,
                    UserId = entity.UserId,
                    //UserName = entity.UserName,
                    UserFirstName = entity.UserFirstName,
                    UserLastName = entity.UserLastName,
                    TrainingId = entity.TrainingId,
                    TrainingName = entity.TrainingName,
                    Status = entity.Status
                });
            }
            return eventsEntityBases;
        }




        // to show Events count for each date on the calendar for current User
        public IDictionary<DateTime, int> GetEventsCountForEachDateByUser(string userId)
        {
            var allEventsByUser = _context.Events.Where(x => x.UserId == userId).OrderBy(x => x.Date).ToList();
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
            var allEventsByTrainer = _context.Events.Where(x => x.TrainerId == trainerId).OrderBy(x => x.Date).ToList();
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
            bool result = changedEntry > 0 ? true : false;

            return result;
        }



        public int GetActualEventsCountByTrainer(string trainerId)
        {
            var actualEventsCount = _context.Events.Where(x => x.TrainerId == trainerId).Where(x => x.Date.Date >= DateTime.Now.Date).ToList().Count();
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
            if (addedRowCount >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }

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
            var allContactsId = _context.ChatContacts.Where(x => x.UserId == userId).Select(x => x.InterlocutorId).ToList();
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
            var message = _context.ChatMessages.Where(x => x.Id == messageId).First();
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

            if (addedContactCount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public IEnumerable<User> GetUsersByRoles(IEnumerable<IdentityRole> roles)
        //{
        //    var userRoleJoin = (from role in _context.UserRoles
        //        join user in _context.Users
        //            on role.RoleId equals user.Id
        //            select new
        //            {

        //            }

        //}
    }
}
