using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.Extensions.Logging;


namespace FitMeApp.Controllers
{
    public sealed class ChartController : Controller
    {
        private readonly ILogger _logger;

        public ChartController(ILogger<ChartController> logger)
        {
            _logger = logger;
        }


        public IActionResult AttendanceChart(int gymId)
        {
            try
            {
                var gymViewModel = new GymViewModel(){Id = gymId};
                return View(gymViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return RedirectToAction("CurrentGymInfo", "Gyms", new { gymId = gymId });
        }
    }
}
