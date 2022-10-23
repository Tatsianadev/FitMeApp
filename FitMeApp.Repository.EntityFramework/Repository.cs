using FitMeApp.Repository.EntityFramework.Entities;
using FitMeApp.Repository.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace FitMeApp.Repository.EntityFramework
{
    public class Repository : IRepository
    {
        ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<GymEntity> GetAllGyms()
        {
            var gyms = _context.Gyms.ToList();
            return gyms;
        }

        public GymEntity GetGym(int id)
        {
            GymEntity gym = _context.Gyms.First(x => x.Id == id);
            return gym;
        }


        public GymEntity AddGym(GymEntity item)
        {
            if (item == null)
            {
                throw new NotImplementedException();
            }

            _context.Add(item);
            _context.SaveChanges();
            return item;
        }

        public bool UpdateGym(int id, GymEntity newItem)
        {
            if (newItem == null)
            {
                throw new NotImplementedException();
            }

            GymEntity gym = _context.Gyms.First(x => x.Id == id);
            gym.Name = newItem.Name;
            gym.Address = newItem.Address;
            gym.Phone = newItem.Address;
            gym.TrainerGymRelation = newItem.TrainerGymRelation;

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

    }
}
