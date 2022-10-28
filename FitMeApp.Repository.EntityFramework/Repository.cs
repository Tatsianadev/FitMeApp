using FitMeApp.Repository.EntityFramework.Entities;
using FitMeApp.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Contracts.BaseEntities;

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
            if (result>0)
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
        public IEnumerable<GroupClassEntityBase> GetAllGroupClasses()
        {
            var groupClasses = _context.GroupClasses.ToList();
            return groupClasses;
        }


        public GroupClassEntityBase GetGroupClass(int id)
        {
            var groupClass = _context.GroupClasses.Where(x => x.Id == id).First();
            return groupClass;
        }
        
        
        public GroupClassEntityBase AddGroupClass(GroupClassEntityBase item)
        {
            if (item == null)
            {
                throw new NotImplementedException();
            }

            _context.Add(new GroupClassEntity() 
            { 
                Name = item.Name,
                Description = item.Description
            });
            _context.SaveChanges();
            return item;
        }


        public bool UpdateGroupClass(int id, GroupClassEntityBase newGroupClassData)
        {
            if (newGroupClassData == null)
            {
                throw new NotImplementedException();
            }

            var groupClass = _context.GroupClasses.Where(x => x.Id == id).First();
            groupClass.Name = newGroupClassData.Name;
            groupClass.Description = newGroupClassData.Description;

            var result = _context.SaveChanges();
            if (result>0)
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
            var groupClass = _context.GroupClasses.Where(x => x.Id == id).First();
            _context.GroupClasses.Remove(groupClass);
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


        //TrainersGym
        public IEnumerable<TrainerEntityBase> GetTrainersOfGym(int gymId)
        {           
            var trainersGym = _context.TrainerGym.Where(x => x.GymId == gymId).ToList();
            var staff = new List<TrainerEntityBase>();

            foreach (var item in trainersGym)
            {
                var trainer = _context.Trainers.Where(x => x.Id == item.TrainerId).First();
                staff.Add(trainer);
            }            
            return staff;
        }

        public IEnumerable<GymEntityBase> GetGymsOfTrainer(int trainerId)
        {
            var trainerGym = _context.TrainerGym.Where(x => x.TrainerId == trainerId).ToList();
            var gyms = new List<GymEntityBase>();

            foreach (var item in trainerGym)
            {
                var gym = _context.Gyms.Where(x => x.Id == item.GymId).First();
                gyms.Add(gym);
            }
            return gyms;
        }

        public IEnumerable<GroupClassEntityBase> GetGroupClassesOfGym(int gymId)
        {
            var groupClassGymTrainer = _context.GroupClassGym.Where(x => x.GymId == gymId).ToList();
            var groupClasses = new List<GroupClassEntity>();
            foreach (var item in groupClassGymTrainer)
            {
                var groupClass = _context.GroupClasses.Where(x => x.Id == item.GroupClassId).First();
                groupClasses.Add(groupClass);
            }
            return groupClasses;
        }

        public IEnumerable<GroupClassEntityBase> GetGroupClassesOfTrainer(int trainerId)
        {
            var groupClassGymTrainer = _context.GroupClassGym.Where(x => x.TrainerId == trainerId).ToList();
            var groupClasses = new List<GroupClassEntity>();
            foreach (var item in groupClassGymTrainer)
            {
                var groupClass = _context.GroupClasses.Where(x => x.Id == item.GroupClassId).First();
                groupClasses.Add(groupClass);
            }
            return groupClasses;
        }

        public IEnumerable<TrainerEntityBase> GetTrainersOfGroupClass(int groupClassId)
        {
            var groupClassGymTrainers = _context.GroupClassGym.Where(x => x.GroupClassId == groupClassId).ToList();
            var trainers = new List<TrainerEntityBase>();
            foreach (var item in groupClassGymTrainers)
            {
                var trainer = _context.Trainers.Where(x => x.Id == item.TrainerId).First();
                trainers.Add(trainer);
            }
            return trainers;
        }

        public IEnumerable<GymEntityBase> GetGymsOfGroupClass(int groupClassId)
        {
            var groupClassGymTrainers = _context.GroupClassGym.Where(x => x.GroupClassId == groupClassId).ToList();
            var gyms = new List<GymEntityBase>();
            foreach (var item in groupClassGymTrainers)
            {
                var gym = _context.Gyms.Where(x => x.Id == item.GymId).First();
                gyms.Add(gym);
            }
            return gyms;
        }

    }
}
