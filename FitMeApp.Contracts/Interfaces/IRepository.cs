using FitMeApp.Contracts.BaseEntities;
using System.Collections.Generic;


namespace FitMeApp.Contracts.Interfaces
{
    public interface IRepository
    {
        IEnumerable<GymEntityBase> GetAllGyms();
        GymEntityBase GetGym(int id);
        GymEntityBase AddGym(GymEntityBase item);
        bool UpdateGym(int id, GymEntityBase newItem);
        bool DeleteGym(int id);


    }
}
