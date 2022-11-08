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

        //GroupClasses
        public IEnumerable<ClassEntityBase> GetAllGroupClasses()
        {
            var groupClasses = _context.Classes.ToList();
            return groupClasses;
        }


        public ClassEntityBase GetGroupClass(int id)
        {
            var groupClass = _context.Classes.Where(x => x.Id == id).First();
            return groupClass;
        }


        public ClassEntityBase AddGroupClass(ClassEntityBase item)
        {
            if (item == null)
            {
                throw new NotImplementedException();
            }

            _context.Add(new ClassEntity()
            {
                Name = item.Name,
                Description = item.Description
            });
            _context.SaveChanges();
            return item;
        }


        public bool UpdateGroupClass(int id, ClassEntityBase newGroupClassData)
        {
            if (newGroupClassData == null)
            {
                throw new NotImplementedException();
            }

            var groupClass = _context.Classes.Where(x => x.Id == id).First();
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


        public bool DeleteGroupClass(int id)
        {
            var groupClass = _context.Classes.Where(x => x.Id == id).First();
            _context.Classes.Remove(groupClass);
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

        //Gym - Trainer - GroupClass connection

        public GymWithTrainersAndGroupBase GetGymWithStaffAndGroup(int gymId)
        {
            try
            {
                var gymsFullInfo = (from gym in _context.Gyms
                                    join trainer in _context.Trainers
                                    on gym.Id equals trainer.GymId
                                    join classTrainer in _context.ClassTrainer
                                    on trainer.Id equals classTrainer.TrainerId                                     
                                    join Class in _context.Classes
                                    on classTrainer.ClassId equals Class.Id                                   
                                    where gym.Id == gymId
                                    select new
                                    {
                                        GymId = gym.Id,
                                        GymName = gym.Name,
                                        GymAddress = gym.Address,
                                        GymPhone = gym.Phone,
                                        TrainerId = trainer.Id,
                                        TrainerFirstName = trainer.FirstName,
                                        TrainerLastName = trainer.LastName,
                                        TrainerGender = trainer.Gender,
                                        TrainerPicture = trainer.Picture,
                                        TrainerSpecialization = trainer.Specialization,
                                        ClassId = Class.Id,
                                        ClassName = Class.Name,
                                        ClassDescription = Class.Description
                                    }).ToList();

                var trainers = new List<TrainerEntityBase>();
                int trainerId = 0;
                var groupClasses = new List<ClassEntityBase>();
                int classId = 0;
                GymWithTrainersAndGroupBase gymWithTrainersAndClasses = new GymWithTrainersAndGroupBase();

                var gymInfo = gymsFullInfo.First();
                gymWithTrainersAndClasses.Id = gymInfo.GymId;
                gymWithTrainersAndClasses.Name = gymInfo.GymName;
                gymWithTrainersAndClasses.Phone = gymInfo.GymPhone;
                gymWithTrainersAndClasses.Address = gymInfo.GymAddress;
               
                foreach (var item in gymsFullInfo)
                { 
                    if (trainerId != item.TrainerId)
                    {
                        trainers.Add(new TrainerEntityBase()
                        {
                            Id = item.TrainerId,
                            FirstName = item.TrainerFirstName,
                            LastName = item.TrainerLastName,
                            Gender = item.TrainerGender,
                            Picture = item.TrainerPicture,
                            Specialization = item.TrainerSpecialization
                            
                        });
                        trainerId = item.TrainerId;                        
                    }

                    if (classId != item.ClassId)
                    {
                        groupClasses.Add(new ClassEntityBase()
                        {
                            Id = item.ClassId,
                            Name = item.ClassName,
                            Description = item.ClassDescription
                        });
                        classId = item.ClassId;                        
                    }
                }

                gymWithTrainersAndClasses.Trainers = trainers;
                gymWithTrainersAndClasses.GroupClasses = groupClasses;


                return gymWithTrainersAndClasses;
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

        public void gettrainersByGym() { }
        public void getClassesByGym() { }
        public void getTrainersAndClassesbyGym()
        {
            gettrainersByGym();
            getClassesByGym();
            return;
        }



        public TrainerWithGymAndGroupBase GetTrainerWithGymAndGroup(int trainerId)
        {
            var trainer = _context.Trainers.Where(x => x.Id == trainerId).First();
            var gym = _context.Gyms.Where(x => x.Id == trainer.GymId).First();

            var groupClassGymTrainer = _context.ClassTrainer.Where(x => x.TrainerId == trainerId).ToList();
            var groupClasses = new List<ClassEntity>();
            foreach (var item in groupClassGymTrainer)
            {
                var groupClass = _context.Classes.Where(x => x.Id == item.ClassId).First();
                groupClasses.Add(groupClass);
            }

            TrainerWithGymAndGroupBase trainerWithGymAndGroup = new TrainerWithGymAndGroupBase()
            {
                Id = trainer.Id,
                FirstName = trainer.FirstName,
                LastName = trainer.LastName,
                Gender = trainer.Gender,
                Picture = trainer.Picture,
                Specialization = trainer.Specialization,
                Gym = gym,
                GroupClasses = groupClasses
            };

            return trainerWithGymAndGroup;

        }

        public GroupClassWithGymsAndTrainersBase GetGroupClassWithGymsAndTrainers(int groupClassId)
        {
            var groupClassGymTrainers = _context.ClassTrainer.Where(x => x.ClassId == groupClassId).ToList();
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

            var groupClass = _context.Classes.Where(x => x.Id == groupClassId).First();
            GroupClassWithGymsAndTrainersBase groupClassWithGymsAndTrainers = new GroupClassWithGymsAndTrainersBase()
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

        public IEnumerable<GymWithTrainersAndGroupBase> GetGymsOfGroupClasses(List<int> groupClassesId)
        {
            var groupClassGymTrainers = new List<ClassTrainerEntity>();
            foreach (var groupClassId in groupClassesId)
            {
                var blockGroupClassGymTrainers = _context.ClassTrainer.Where(x => x.ClassId == groupClassId).ToList();
                foreach (var item in blockGroupClassGymTrainers)
                {
                    groupClassGymTrainers.Add(item);
                }
            }

            var gyms = new List<GymEntityBase>();
            var gymsWithStaffAndGroups = new List<GymWithTrainersAndGroupBase>();
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
