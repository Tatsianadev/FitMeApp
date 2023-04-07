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
using FitMeApp.Mapper;

namespace FitMeApp.Controllers
{
    public class ChartController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IGymService _gymService;
        //private readonly ITrainerService _trainerService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly ModelViewModelMapper _mapper;


        public ChartController(IFileService fileService, IGymService gymService, UserManager<User> userManager, ILogger<ChartController> logger)
        {
            _fileService = fileService;
            _gymService = gymService;
            //_trainerService = trainerService;
            _userManager = userManager;
            _logger = logger;
            _mapper = new ModelViewModelMapper();
        }


        public async Task<IActionResult> VisitingChart(int gymId)
        {
            try
            {
                //todo create model without Db (new)
                //var visitingChartModels = _gymService.GetVisitingChartDataForCertainDayByGym(gymId);
                //var viewModel = new List<VisitingChartViewModel>();
                //foreach (var model in visitingChartModels)
                //{
                //    viewModel.Add(_mapper.MapVisitingChartModelToViewModel(model));
                //}

                var viewModel = new VisitingChartViewModel()
                {
                    GymId = gymId
                };

                return View(viewModel);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return RedirectToAction("CurrentGymInfo", "Gyms", new { gymId = gymId });
        }


        //public IActionResult VisitingChartForSelectedDayOfWeek(int gymId, DayOfWeek selectedDay)
        //{
        //    try
        //    {
        //        var visitingChartModel = _gymService.GetVisitingChartDataForCertainDayByGym(gymId, selectedDay);
        //        var viewModel = _mapper.MapVisitingChartModelToViewModel(visitingChartModel);

        //        return ViewComponent("VisitingChartBySelectedDayOfWeek", new { model = viewModel });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //    }

        //    return RedirectToAction("VisitingChart", new {gymId = gymId});
        //}


    }
}
