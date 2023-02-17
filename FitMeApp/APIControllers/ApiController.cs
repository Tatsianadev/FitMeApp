﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.Mapper;
using FitMeApp.Common;

namespace FitMeApp.APIControllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IFitMeService _fitMeService;
        private readonly ModelViewModelMapper _mapper;

        public ApiController(ITrainingService trainingService, IFitMeService fitMeService)
        {
            _trainingService = trainingService;
            _fitMeService = fitMeService;
            _mapper = new ModelViewModelMapper();
        }


        [HttpPost]
        [Route("getavailabletime")]
        public IEnumerable<string> GetAvailableTime(string trainerId, DateTime selectedDate)
        {
            List<string> stringTime = new List<string>();

            var workDays = _fitMeService.GetWorkHoursByTrainer(trainerId).Select(x => x.DayName).ToList();
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

    }
}