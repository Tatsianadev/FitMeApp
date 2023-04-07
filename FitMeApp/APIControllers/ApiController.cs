using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Mapper;
using FitMeApp.Common;
using FitMeApp.WEB.Contracts.ViewModels.Chart;
using Newtonsoft.Json;

namespace FitMeApp.APIControllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly ModelViewModelMapper _mapper;

        public ApiController(ITrainingService trainingService, IGymService gymService, ITrainerService trainerService)
        {
            _trainingService = trainingService;
            _gymService = gymService;
            _trainerService = trainerService;
            _mapper = new ModelViewModelMapper();
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
        [Route("getvisitingchartline")]
        public string GetVisitingChartLine(int gymId, DayOfWeek selectedDay)
        {
            var timeVisitorsLineJson = "";
            try
            {
                var visitingChartModel = _gymService.GetVisitingChartDataForCertainDayByGym(gymId, selectedDay);
                var viewModel = _mapper.MapVisitingChartModelToViewModel(visitingChartModel);
                timeVisitorsLineJson = JsonConvert.SerializeObject(viewModel.TimeVisitorsLine);
            }
            catch (Exception ex)
            {
               
            }

            return timeVisitorsLineJson;
        }

    }
}
