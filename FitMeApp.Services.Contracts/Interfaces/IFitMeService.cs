using FitMeApp.Services.Contracts.Models;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFitMeService
    {
        IEnumerable<GymModel> GetAllGymModels();
        GymModel GetGymModel(int id);
        IEnumerable<TrainerModel> GetAllTrainerModels();
        ICollection<TrainingModel> GetAllTrainingModels();
        IEnumerable<GymModel> GetGymsByTrainings(List<int> groupClassesId);
    }
}
