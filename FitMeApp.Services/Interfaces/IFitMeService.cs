using FitMeApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Interfaces
{
    public interface IFitMeService
    {
        IEnumerable<GymModel> GetAllGymModels();
        IEnumerable<TrainerModel> GetAllTrainerModels();
        ICollection<GroupClassModel> GetAllGroupClassModels();
    }
}
