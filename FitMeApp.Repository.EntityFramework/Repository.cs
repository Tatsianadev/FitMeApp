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


        public GymEntityBase AddGym(GymEntityBase item)
        {
            if (item == null)
            {
                throw new NotImplementedException();
            }

            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public bool UpdateGym(int id, GymEntityBase newItem)
        {
            if (newItem == null)
            {
                throw new NotImplementedException();
            }

            GymEntity gym = _context.Gyms.First(x => x.Id == id);

            gym.Name = newItem.Name;
            gym.Address = newItem.Address;
            gym.Phone = newItem.Address;           

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

    }
}
