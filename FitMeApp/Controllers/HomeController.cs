using FitMeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services;
using FitMeApp.Services.Interfaces;

namespace FitMeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFitMeService _fitMeService;
        private readonly ILogger<HomeController> _logger;       

        public HomeController(IFitMeService fitMeService, ILogger<HomeController> logger)
        {
            _fitMeService = fitMeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            try
            {
                var gyms = _fitMeService.GetAllGymModels();
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }

          
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
