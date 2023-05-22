using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FitMeApp.APIControllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly ILogger _logger;

        public ApiController(ITrainingService trainingService, IGymService gymService, ITrainerService trainerService, ILogger<ApiController> logger)
        {
            _trainingService = trainingService;
            _gymService = gymService;
            _trainerService = trainerService;
            _logger = logger;
        }


        [HttpPost]
        [Route("getavailabletime")]
        public IEnumerable<string> GetAvailableTime(string trainerId, DateTime selectedDate)
        {
            List<string> stringTime = new List<string>();

            var workDays = _trainerService.GetWorkHoursByTrainer(trainerId).Select(x => x.DayName).ToList();
            if (workDays.Contains(selectedDate.DayOfWeek))
            {
                List<int> availableTimeInMinutes = _trainingService
                    .GetAvailableTimeForTraining(trainerId, selectedDate)
                    .ToList();

                foreach (var intTime in availableTimeInMinutes)
                {
                    stringTime.Add(WorkHoursTypesConverter.ConvertIntTimeToString(intTime));
                }
            }
            else
            {
                ModelState.AddModelError("The selected day is dayOff for a current trainer. Please, select another selectedDate", "Selected day is dayOff");
            }

            return stringTime;
        }


        [HttpPost]
        [Route("getattendancechartline")]
        public string GetVisitingChartLine(int gymId, DayOfWeek selectedDay)
        {
            var timeVisitorsLineJsonString = "";
            try
            {
                var visitingChartModel = _gymService.GetAttendanceChartDataForCertainDayByGym(gymId, selectedDay);
                if (visitingChartModel != null)
                {
                    timeVisitorsLineJsonString = JsonConvert.SerializeObject(visitingChartModel.NumberOfVisitorsPerHour);
                }
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, ex.Message);
            }

            return timeVisitorsLineJsonString;
        }

        [HttpPost]
        [Route("checkselectedtimefordays")]
        public bool CheckIfSelectedTimeAvailableForDays(string trainerId, string selectedTime,
            List<string> selectedDaysOfWeek)
        {
            return false;
        }

    }
}
