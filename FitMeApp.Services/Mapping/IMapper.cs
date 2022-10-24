using FitMeApp.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Mapping
{
    public interface IMapper
    {
        GymModel GetGymModel(int id);
        TrainerModel GetTrainerModel(int id);
        GroupClassModel GetGroupClassModel(int id);
    }
}
