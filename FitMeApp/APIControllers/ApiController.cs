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
        private readonly IScheduleService _scheduleService;
        private readonly ILogger _logger;

        public ApiController(ITrainingService trainingService, IGymService gymService, ITrainerService trainerService, IScheduleService scheduleService, ILogger<ApiController> logger)
        {
            _trainingService = trainingService;
            _gymService = gymService;
            _trainerService = trainerService;
            _scheduleService = scheduleService;
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
            List<string> selectedDaysOfWeek, int duration = 60)
        {
            if (selectedTime == null)
            {
                return false;
            }
            int startTime = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(selectedTime);
            var datesToCheck = new List<DateTime>();

            foreach (var day in selectedDaysOfWeek)
            {
                datesToCheck.AddRange(GetDatesInSpanByDayOfWeek(day,30));
            }

            //todo continue implementation
            //GetEventsCount(trainerId, dates, time)
            int endTime = startTime + duration;
            int eventsCount = _scheduleService.GetEventsCount(trainerId, datesToCheck, startTime, endTime);
            //GetGroupClassesCount(trainerId, dates, time)

            //if count == 0 => true, else => false
             

            return false;
        }

        private IEnumerable<DateTime> GetDatesInSpanByDayOfWeek(string day, int daysSpan)
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;
            int dayNumber = DateTime.Now.Day;

            List<DateTime> dates = Enumerable.Range(1, daysSpan)
                .Where(d => new DateTime(year, month, dayNumber).AddDays(d-1).ToString("dddd").Equals(day))
                .Select(d => new DateTime(year, month, dayNumber).AddDays(d-1)).ToList();

            return dates;
        }
        

    }
}
