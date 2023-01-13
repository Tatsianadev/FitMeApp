﻿using FitMeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
//using ASP;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;

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
                //throw  new  Exception("test logger");
               
            }
            catch (Exception ex)
            {
                _logger.LogError("test logger" + DateTime.Now, ex.Message);
               
                //throw ex;
            }
            return View();
        }



        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
