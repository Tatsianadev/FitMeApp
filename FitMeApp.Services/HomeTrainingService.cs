using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EllipticCurve.Utils;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using File = System.IO.File;
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
            var jsonString = await GetJsonResponseAsStringAsync("allHomeTrainings");
            var homeTrainingsModels = ConvertJsonResponseToHomeTrainingModels(jsonString);
            return homeTrainingsModels;
        }

        
        public async Task<IEnumerable<HomeTrainingModel>> GetHomeTrainingsByFilterAsync(string gender, int age,
            int calorie, int duration, bool equipment)
        {
            StringBuilder endpointBuilder = new StringBuilder("homeTrainingsByFilter?");

            if (gender != Common.GenderEnum.all.ToString() && gender != null)
            {
                endpointBuilder.Append($"gender={gender}&");
            }

            if (age != 0)
            {
                endpointBuilder.Append($"age={age}&");
            }

            if (calorie != 0)
            {
                endpointBuilder.Append($"calorie={calorie}&");
            }

            if (duration != 0)
            {
                endpointBuilder.Append($"duration={duration}&");
            }

            if (equipment != true)
            {
                endpointBuilder.Append("equipment=0&");
            }

            string endpointString = endpointBuilder.ToString();
            var resultEndpoint = endpointString.Remove(endpointString.Length-1, 1);
            var jsonString = await GetJsonResponseAsStringAsync(resultEndpoint);
            if (jsonString == string.Empty)
            {
                return new List<HomeTrainingModel>();
            }
            var homeTrainingsModels = ConvertJsonResponseToHomeTrainingModels(jsonString);
            return homeTrainingsModels;
        }

        public async Task<byte[]> DownloadPdfFileAsync(int homeTrainingPlanId)
        {
            string endpoint = "returnfilepdf/" + homeTrainingPlanId;
            byte[] response = await GetResponseAsByteArrayAsync(endpoint);
            return response;
        }


        private IEnumerable<HomeTrainingModel> ConvertJsonResponseToHomeTrainingModels(string jsonString)
        {
            var homeTrainingsDict = JsonConvert.DeserializeObject<Dictionary<int, HomeTrainingModel>>(jsonString);
            var homeTrainingsModels = homeTrainingsDict.Values;
            return homeTrainingsModels;
        }


        private async Task<string> GetJsonResponseAsStringAsync(string endpoint)
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

        private async Task<byte[]> GetResponseAsByteArrayAsync(string endpoint)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                byte[] responseContent = await response.Content.ReadAsByteArrayAsync();
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
