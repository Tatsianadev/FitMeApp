using FitMeApp.Repository.EntityFramework.Entities;
using System.Collections.Generic;


namespace FitMeApp.Repository.EntityFramework.Interfaces
{
    public interface IRepository
    {
        IEnumerable<GymEntity> GetAllGyms();
        GymEntity GetGym(int id);
        GymEntity AddGym(GymEntity item);
        bool UpdateGym(int id, GymEntity newItem);
        bool DeleteGym(int id);


    }
}
