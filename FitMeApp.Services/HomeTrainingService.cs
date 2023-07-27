using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace FitMeApp.Services
{
    public class HomeTrainingService : IHomeTrainingService
    {
        private readonly ILogger<HomeTrainingService> _logger;
        private readonly HttpClient httpClient;

        public HomeTrainingService(ILogger<HomeTrainingService> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://127.0.0.1:5000/");
        }

        public async Task<IEnumerable<HomeTrainingModel>> GetAllHomeTrainingsAsync()
        {
            var jsonString = await GetJsonResponseAsync("allHomeTrainings");
            var homeTrainingsDict = JsonConvert.DeserializeObject<Dictionary<int, HomeTrainingModel>>(jsonString);
            var homeTrainingsModels = homeTrainingsDict.Values;
            return homeTrainingsModels;
        }


        private async Task<string> GetJsonResponseAsync(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                return responseContent;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
