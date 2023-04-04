using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.WEB.Contracts.ViewModels.Chart;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using FitMeApp.Common;
using Microsoft.AspNetCore.Authorization;

namespace FitMeApp.Controllers
{
    public class ChartController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IGymService _gymService;
        //private readonly ITrainerService _trainerService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public ChartController(IFileService fileService, IGymService gymService, UserManager<User> userManager, ILogger<ChartController> logger)
        {
            _fileService = fileService;
            _gymService = gymService;
            //_trainerService = trainerService;
            _userManager = userManager;
            _logger = logger;
        }


        public async Task<IActionResult> VisitingChart(int gymId)
        {
            try
            {
                var gym = _gymService.GetGymModel(gymId);
                List<VisitingChartViewModel> visitingData = new List<VisitingChartViewModel>();
                string fullPath = Environment.CurrentDirectory + @"\wwwroot\ExcelFiles\Import\VisitorsChart.xlsx";
                var visitingChartModels = await _fileService.ReadFromExcel(fullPath);
                foreach (var model in visitingChartModels)
                {
                    var timeVisitorsViewModel = new List<TimeVisitorsAsChartDataPointViewModel>();
                    foreach (var point in model.TimeVisitorsLine)
                    {
                        timeVisitorsViewModel.Add(new TimeVisitorsAsChartDataPointViewModel()
                        {
                            NumberOfVisitors = point.NumberOfVisitors,
                            Hour = point.Hour.ToString() + ".00"
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

               // ViewBag.SelectedDay = DayOfWeek.Monday.ToString();
              
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
