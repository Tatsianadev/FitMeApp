using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Controllers
{
    public class HomeTraining : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient httpClient;

        public HomeTraining(ILogger<HomeController> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://127.0.0.1:5000/");
        }

        private async Task<string> GetPythonApiDataAsync()
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync("test");
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ex.Message;
            }
        }


        public async Task<IActionResult> TestPyAPI()
        {
            var response =await GetPythonApiDataAsync();
            return View("TestPyAPI", response);
        }
       
    }
}
