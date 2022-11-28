﻿using FitMeApp.Repository.EntityFramework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;

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
            gym.Phone = newGymData.Address;

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

        //Trainers//

        public IEnumerable<TrainerEntityBase> GetAllTrainers()
        {
            var trainers = _context.Trainers.ToList();
            return trainers;
        }

        public TrainerEntityBase GetTrainer(int id)
        {
            var trainer = _context.Trainers.Where(x => x.Id == id).First();
            return trainer;
        }

        public TrainerEntityBase AddTrainer(TrainerEntityBase trainer)
        {
            if (trainer == null)
            {
                throw new NotImplementedException();
            }

            _context.Trainers.Add(new TrainerEntity()
            {
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization
            });

            _context.SaveChanges();
            return trainer;
        }

        public bool UpdateTrainer(int id, TrainerEntityBase newTrainerData)
        {
            if (newTrainerData == null)
            {
                throw new NotImplementedException();
            }

            var trainer = _context.Trainers.Where(x => x.Id == id).First();
            trainer.FirstName = newTrainerData.FirstName;
            trainer.LastName = newTrainerData.LastName;
            trainer.Gender = newTrainerData.Gender;
            trainer.Picture = newTrainerData.Picture;
            trainer.Specialization = newTrainerData.Specialization;

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


        public bool DeleteTrainer(int id)
        {
            var trainer = _context.Trainers.Where(x => x.Id == id).First();
            _context.Trainers.Remove(trainer);
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


        public bool DeleteTraining(int id)
        {
            var groupClass = _context.Trainings.Where(x => x.Id == id).First();
            _context.Trainings.Remove(groupClass);
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

        //Gym - Trainer - Trainings connection

        public GymWithTrainersAndTrainings GetGymWithTrainersAndTrainings(int gymId)
        {
            try
            {
                var gymTrainerTrainingJoin = (from gymDb in _context.Gyms
                                              join trainer in _context.Trainers
                                              on gymDb.Id equals trainer.GymId
                                              join trainingTrainer in _context.TrainingTrainer
                                              on trainer.Id equals trainingTrainer.TrainerId
                                              join training in _context.Trainings
                                              on trainingTrainer.TrainingId equals training.Id
                                              where gymDb.Id == gymId
                                              select new
                                              {
                                                  GymId = gymDb.Id,
                                                  GymName = gymDb.Name,
                                                  GymAddress = gymDb.Address,
                                                  GymPhone = gymDb.Phone,
                                                  TrainerId = trainer.Id,
                                                  TrainerFirstName = trainer.FirstName,
                                                  TrainerLastName = trainer.LastName,
                                                  TrainerGender = trainer.Gender,
                                                  TrainerPicture = trainer.Picture,
                                                  TrainerSpecialization = trainer.Specialization,
                                                  TrainingId = training.Id,
                                                  TrainingName = training.Name,
                                                  TrainingDescription = training.Description
                                              }).ToList();

                var trainers = new List<TrainerWithGymAndTrainingsBase>();
                List<int> addedTrainersId = new List<int>();

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
                            Trainings = trainings
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
                                               join gym in _context.Gyms
                                               on trainer.GymId equals gym.Id
                                               join trainingTrainer in _context.TrainingTrainer
                                               on trainer.Id equals trainingTrainer.TrainerId
                                               join training in _context.Trainings
                                               on trainingTrainer.TrainingId equals training.Id
                                               select new
                                               {
                                                   TrainerId = trainer.Id,
                                                   FirstName = trainer.FirstName,
                                                   LastName = trainer.LastName,
                                                   Gender = trainer.Gender,
                                                   Picture = trainer.Picture,
                                                   Specialization = trainer.Specialization,
                                                   GymId = gym.Id,
                                                   GymName = gym.Name,
                                                   GymAddress = gym.Address,
                                                   TrainingId = training.Id,
                                                   TrainingName = training.Name
                                               }).OrderBy(x => x.TrainerId).ToList();

            
            List<TrainerWithGymAndTrainingsBase> trainersWithGymAndTrainings = new List<TrainerWithGymAndTrainingsBase>();
            List<int> trainersId = new List<int>();
            var trainerWithGymAndTrainings = new TrainerWithGymAndTrainingsBase();

            foreach (var item in allTrainersGymTrainingsJoin)
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
                    //trainings.Clear();

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



        public TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(int trainerId)
        {

            return null;

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


        public IEnumerable<SubscriptionPriceBase> GetSubscriptionsByGymByFilter(int gymId, List<int> periods, bool groupTraining, bool dietMonitoring)
        {
            var subscriptionsPriceJoinByGymByFilters = (from gymSubscription in _context.GymSubscriptions
                                                        join subscription in _context.Subscriptions
                                                        on gymSubscription.SubscriptionId equals subscription.Id
                                                        where gymSubscription.GymId == gymId
                                                        where periods.Contains(subscription.ValidDays)
                                                        where subscription.GroupTraining == groupTraining
                                                        where subscription.DietMonitoring == dietMonitoring
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
            foreach (var subscription in subscriptionsPriceJoinByGymByFilters)
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

        // Subscribtions

        public IEnumerable<SubscriptionPriceBase> GetAllSubscriptionsByGym(int gymId)
        {
            var subscriptionsPriceJoinByGym = (from gymSubscription in _context.GymSubscriptions
                                               join subscription in _context.Subscriptions
                                               on gymSubscription.SubscriptionId equals subscription.Id
                                               where gymSubscription.GymId == gymId
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
            foreach (var subscription in subscriptionsPriceJoinByGym)
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
            int period = _context.Subscriptions.Where(x => x.Id == subscriptionId).First().ValidDays;
            return period;
        }


        public SubscriptionPriceBase GetSubscriptionByGym(int subscriptionId, int gymId)
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

            SubscriptionPriceBase currentSubscription = new SubscriptionPriceBase()
            {
                Id = subscriptionGymJoin.SubscriptionId,
                GymId = subscriptionGymJoin.GymId,
                ValidDays = subscriptionGymJoin.ValidDays,
                GroupTraining = subscriptionGymJoin.GroupTraining,
                DietMonitoring = subscriptionGymJoin.DietMonitoring,
                Price = subscriptionGymJoin.Price
            };
            return currentSubscription;
        }



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

        //public IEnumerable<EventEntityBase> GetEventsByUserAndDate(string userId, DateTime dateTime)
        //{          
        //    string dateOnly = dateTime.ToString("yyyy-MM-dd");
        //    var userEvents = _context.Events.Where(x => x.UserId == userId && x.Date.ToString() == dateOnly).OrderBy(x=>x.StartTime).ToList();
        //    return userEvents;
        //}

        public IEnumerable<EventWithNamesBase> GetEventsByUserAndDate(string userId, DateTime dateTime)
        {
            string dateOnly = dateTime.ToString("yyyy-MM-dd");

            var eventJoinTrainerTrainingUserGym = (from events in _context.Events
                                                   join user in _context.Users
                                                   on events.UserId equals user.Id
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
                                                       TrainerFirstName = trainer.FirstName,
                                                       TrainerLastName = trainer.LastName,
                                                       GymId = trainer.GymId,
                                                       GymName = gym.Name,
                                                       UserId = events.UserId,
                                                       UserName = user.UserName,
                                                       TrainingId = events.TrainingId,
                                                       TrainingName = training.Name,
                                                       Status = events.Status})
                                                       .OrderBy(x=>x.StartTime).ToList();

            List<EventWithNamesBase> eventsEntityBases = new List<EventWithNamesBase>();
            foreach (var entity in eventJoinTrainerTrainingUserGym)
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
                    UserName = entity.UserName,
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


    }
}
