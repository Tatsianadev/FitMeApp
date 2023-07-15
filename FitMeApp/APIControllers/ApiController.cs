using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Common;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting.Hosting;

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
        public bool CheckIfSelectedTimeAvailableForDays(string trainerId, string selectedTime, DateTime selectedDate,
            List<string> selectedDaysOfWeek, int duration = 60)
        {
            if (selectedTime == null)
            {
                return false;
            }

            if (selectedDate <= DateTime.Today)
            {
                selectedDate = DateTime.Today.AddDays(1);
            }

            int startTime = Common.WorkHoursTypesConverter.ConvertStringTimeToInt(selectedTime);
            int endTime = startTime + duration;

            //if selected time out of workHours range -> return false
            var trainerWorkHours = _trainerService.GetWorkHoursByTrainer(trainerId).ToList();
            foreach (var selectedDayOfWeek in selectedDaysOfWeek)
            {
                foreach (var item in trainerWorkHours)
                {
                    if (selectedDayOfWeek == item.DayName.ToString() && 
                        (startTime < item.StartTime || endTime > item.EndTime))
                    {
                        return false;
                    }
                }
            }
            
            //if selected time at the same time with event or groupClass -> return false
            var datesToCheck = new List<DateTime>();
            foreach (var dayOfWeekName in selectedDaysOfWeek)
            {
                datesToCheck.AddRange(Common.DateManager.GetDatesInSpanByDayOfWeek(selectedDate, 30, dayOfWeekName));
            }

            int eventsCount = _scheduleService.GetEventsCount(trainerId, datesToCheck, startTime, endTime);
            if (eventsCount == 0)
            {
                int groupClassScheduleRecordsCount = _trainingService.GetGroupClassScheduleRecordsCount(trainerId, datesToCheck, startTime, endTime);
                if (groupClassScheduleRecordsCount == 0)
                {
                    return true;
                }
            }

            return false;
        }


        [HttpPost]
        [Route("getproductsbystartwith")]
        public IEnumerable<string> GetProductsByStartWith(string letters)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            string fullPathToFile = @"c:\tatsiana\projects\FitMeApp\FitMeApp\wwwroot\Python\DietJournal.py"; //todo put path to Resources 
            engine.ExecuteFile(fullPathToFile, scope);

            var function = scope.GetVariable("findNamesByStartWith");
            string allProductsFile = scope.GetVariable("path");
            var resultPy = function(allProductsFile, letters);

            var result = new List<string>();
            foreach (var product in resultPy)
            {
                result.Add(product);
            }
            return result;
        }
    }
}
