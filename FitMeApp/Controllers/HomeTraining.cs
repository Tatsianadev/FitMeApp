using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Controllers
{
    public class HomeTraining : Controller
    {
        private readonly IHomeTrainingService _homeTrainingService;
        private readonly ILogger<HomeController> _logger;

        //private readonly HttpClient httpClient;

        public HomeTraining(IHomeTrainingService homeTrainingService, ILogger<HomeController> logger)
        {
            _homeTrainingService = homeTrainingService;
            _logger = logger;
            //httpClient = new HttpClient();
            //httpClient.BaseAddress = new Uri("http://127.0.0.1:5000/");
        }

        //private async Task<string> GetPythonApiDataAsync()
        //{
        //    try
        //    {
        //        HttpResponseMessage response = await httpClient.GetAsync("test");
        //        response.EnsureSuccessStatusCode();
        //        string responseContent = await response.Content.ReadAsStringAsync();
        //        return responseContent;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, ex.Message);
        //        return ex.Message;
        //    }
        //}


        public async Task<IActionResult> TestPyAPI()
        {
            var response = await _homeTrainingService.GetAllHomeTrainingsAsync();
            return View("TestPyAPI");
        }
       
    }
}
