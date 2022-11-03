using FitMeApp.Services.Contracts.Models;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFitMeService
    {
        IEnumerable<GymModel> GetAllGymModels();
        IEnumerable<TrainerModel> GetAllTrainerModels();
        ICollection<GroupClassModel> GetAllGroupClassModels();
        IEnumerable<GymModel> GetGymsOfGroupClasses(List<int> groupClassesId);
    }
}
