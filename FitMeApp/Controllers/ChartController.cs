﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.WEB.Contracts.ViewModels.Chart;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Controllers
{
    public class ChartController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IGymService _gymService;
        private readonly ILogger _logger;

        public ChartController(IFileService fileService, IGymService gymService, ILogger<ChartController> logger)
        {
            _fileService = fileService;
            _gymService = gymService;
            _logger = logger;
        }


        public async Task<IActionResult> VisitingChart(int gymId)
        {
            try
            {
                var gym = _gymService.GetGymModel(gymId);
                List<VisitingChartViewModel> visitingData = new List<VisitingChartViewModel>();
                string fullPath = Environment.CurrentDirectory + @"\wwwroot\ExcelFiles\Import\BigRock\VisitorsChart.xlsx";
                var visitingChartModels = await _fileService.ReadFromExcel(fullPath);
                foreach (var model in visitingChartModels)
                {
                    var timeVisitorsViewModel = new List<TimeVisitorsViewModel>();
                    foreach (var point in model.TimeVisitorsLine)
                    {
                        timeVisitorsViewModel.Add(new TimeVisitorsViewModel()
                        {
                            Hour = point.Hour,
                            NumberOfVisitors = point.NumberOfVisitors
                        });
                    }

                    var visitingViewModel = new VisitingChartViewModel()
                    {
                        GymId = gymId,
                        GymName = gym.Name,
                        DayOfWeek = model.DayOfWeek,
                        TimeVisitorsLine = timeVisitorsViewModel
                    }; 

                    visitingData.Add(visitingViewModel);
                }

                return View(visitingData);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return RedirectToAction("CurrentGymInfo", "Gyms", new {gymId = gymId});
        }
    }
}