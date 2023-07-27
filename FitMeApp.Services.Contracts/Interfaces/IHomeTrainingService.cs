using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IHomeTrainingService
    {
        Task<IEnumerable<HomeTrainingModel>> GetAllHomeTrainingsAsync();

        Task<IEnumerable<HomeTrainingModel>> GetHomeTrainingsByFilterAsync(string gender, int age,
            int calorie, int duration, bool equipment);
    }
}
