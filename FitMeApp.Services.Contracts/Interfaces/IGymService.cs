using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.Services.Contracts.Models.Chart;
using System;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IGymService
    {
        //Gyms
        IEnumerable<GymModel> GetAllGymModels();
        IEnumerable<GymModel> GetAllGymsWithGalleryModels();
        GymModel GetGymModel(int id);
        IEnumerable<GymModel> GetGymsByTrainings(List<int> groupClassesId);
        IEnumerable<GymWorkHoursModel> GetWorkHoursByGym(int gymId);
        int GetGymWorkHoursId(int gymId, DayOfWeek dayOfWeek);
        int GetGymIdByTrainer(string trainerId);

        //Charts
        AttendanceChartModel GetAttendanceChartDataForCertainDayByGym(int gymId, DayOfWeek day);
    }
}
