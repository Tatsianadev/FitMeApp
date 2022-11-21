using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ILogger _logger;

        public ScheduleController(IScheduleService scheduleService, LoggerFactory loggerFactory)
        {
            _scheduleService = scheduleService;
            _logger = loggerFactory.CreateLogger("ScheduleController");
        }




        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Calendar()
        {
            List<MonthViewModel> months = new List<MonthViewModel>();
            for (int i = 1; i < 13; i++)
            {
                months.Add(new MonthViewModel()
                {
                    Id = i,
                    Name = _scheduleService.GetMonthName(i),
                    DaysNumber = _scheduleService.GetMonthDaysNumber(i)
                });
            }


            return PartialView();
        }
    }
}
