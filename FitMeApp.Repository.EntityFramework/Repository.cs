using FitMeApp.Repository.EntityFramework.Entities;
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
            var groupClasses = _context.Trainings.ToList();
            return groupClasses;
        }


        public TrainingEntityBase GetTraining(int id)
        {
            var groupClass = _context.Trainings.Where(x => x.Id == id).First();
            return groupClass;
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








            //var gym = _context.Gyms.Where(x => x.Id == gymId).First();

            //var trainersGym = _context.Trainers.Where(x => x.GymId == gymId).ToList();
            //var staff = new List<TrainerEntityBase>();
            //foreach (var trainer in trainersGym)
            //{
            //    staff.Add(trainer);
            //}

            //var groupClassGymTrainer = _context.GroupClassTrainer.Where(x => x.GymId == gymId).ToList();
            //var groupClasses = new List<GroupClassEntity>();
            //foreach (var item in groupClassGymTrainer)
            //{
            //    var groupClass = _context.GroupClasses.Where(x => x.Id == item.GroupClassId).First();
            //    if (!groupClasses.Contains(groupClass))
            //    {
            //        groupClasses.Add(groupClass);
            //    }               
            //}

            //GymWithStaffAndGroupBase gymWithStaffAndGroup = new GymWithStaffAndGroupBase()
            //{
            //    Id = gym.Id,
            //    Name = gym.Name,
            //    Address = gym.Address,
            //    Phone = gym.Phone,
            //    Trainers = staff,
            //    GroupClasses = groupClasses
            //};

            //return gymWithStaffAndGroup;
        }

     



        public TrainerWithGymAndTrainingsBase GetTrainerWithGymAndTrainings(int trainerId)
        {
            var trainer = _context.Trainers.Where(x => x.Id == trainerId).First();
            var gym = _context.Gyms.Where(x => x.Id == trainer.GymId).First();

            var groupClassGymTrainer = _context.TrainingTrainer.Where(x => x.TrainerId == trainerId).ToList();
            var groupClasses = new List<TrainingEntity>();
            foreach (var item in groupClassGymTrainer)
            {
                var groupClass = _context.Trainings.Where(x => x.Id == item.TrainingId).First();
                groupClasses.Add(groupClass);
            }

            TrainerWithGymAndTrainingsBase trainerWithGymAndGroup = new TrainerWithGymAndTrainingsBase()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization,
                Gym = gym,
                Trainings = groupClasses
            };

            return trainerWithGymAndGroup;

        }

        public TrainingWithTrainerAndGymBase GetTrainingWithTrainerAndGym(int groupClassId)
        {
            var groupClassGymTrainers = _context.TrainingTrainer.Where(x => x.TrainingId == groupClassId).ToList();
            var trainers = new List<TrainerEntityBase>();
            foreach (var item in groupClassGymTrainers)
            {
                var trainer = _context.Trainers.Where(x => x.Id == item.TrainerId).First();
                trainers.Add(trainer);
            }

            var gyms = new List<GymEntityBase>();
            //foreach (var item in groupClassGymTrainers)
            //{
            //    var gym = _context.Gyms.Where(x => x.Id == item.GymId).First();
            //    if (gyms.Contains(gym))
            //    {
            //        gyms.Add(gym);
            //    }
            //}

            var groupClass = _context.Trainings.Where(x => x.Id == groupClassId).First();
            TrainingWithTrainerAndGymBase groupClassWithGymsAndTrainers = new TrainingWithTrainerAndGymBase()
            {
                Id = groupClass.Id,
                Name = groupClass.Name,
                Description = groupClass.Description,
                Gyms = gyms,
                Trainers = trainers
            };

            return groupClassWithGymsAndTrainers;
        }




        //Filters

        public IEnumerable<GymWithTrainersAndTrainings> GetGymsByTrainings(List<int> groupClassesId)
        {
            var groupClassGymTrainers = new List<TrainingTrainerEntity>();
            foreach (var groupClassId in groupClassesId)
            {
                var blockGroupClassGymTrainers = _context.TrainingTrainer.Where(x => x.TrainingId == groupClassId).ToList();
                foreach (var item in blockGroupClassGymTrainers)
                {
                    groupClassGymTrainers.Add(item);
                }
            }

            var gyms = new List<GymEntityBase>();
            var gymsWithStaffAndGroups = new List<GymWithTrainersAndTrainings>();
            //foreach (var item in groupClassGymTrainers)
            //{
            //    var gym = _context.Gyms.Where(x => x.Id == item.GymId).First();
            //    if (!gyms.Contains(gym))
            //    {
            //        gyms.Add(gym);
            //        gymsWithStaffAndGroups.Add(GetGymWithStaffAndGroup(gym.Id));
            //    }
            //}
            return gymsWithStaffAndGroups;
        }




    }
}
