using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels.Schedule;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class ScheduleController : Controller
    {
        private readonly IScheduleService _scheduleService;
        private readonly ILogger _logger;

        public ScheduleController(IScheduleService scheduleService, ILoggerFactory loggerFactory)
        {
            _scheduleService = scheduleService;
            _logger = loggerFactory.CreateLogger("ScheduleController");
        }




        public IActionResult Index()
        {
            //List<MonthViewModel> months = new List<MonthViewModel>();
            //for (int i = 1; i < 13; i++)
            //{
            //    months.Add(new MonthViewModel()
            //    {
            //        Id = i,
            //        Name = _scheduleService.GetMonthName(i),
            //        DaysNumber = _scheduleService.GetMonthDaysNumber(i),
            //        FirstDayName = _scheduleService.GetFirstDayName(i),
            //        LastDayName = _scheduleService.GetLastDayName(i)
            //    });
            //}
            ViewBag.MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Today.Month);
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return View();

            
        }

        public IActionResult Calendar()
        {
            ViewBag.DaysOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.AbbreviatedDayNames;
            return PartialView();
        }
    }
}
