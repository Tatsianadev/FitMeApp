using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface ITrainingService
    {
        IEnumerable<int> GetAvailableToApplyTrainingTimingByTrainer(string trainerId, DateTime date);
    }
}
