using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.Extensions.Caching.Distributed;

namespace FitMeApp.Controllers
{
    public class CounterController : Controller
    {
        private readonly IDistributedCache _cache;
        private readonly IGymService _gymService;
        private readonly ModelViewModelMapper _mapper;

        public CounterController(IDistributedCache cache, IGymService gymService)
        {
            _cache = cache;
            _gymService = gymService;
            _mapper = new ModelViewModelMapper();
        }
        public async Task<IActionResult> GetGym(int id)
        {
            string message = string.Empty;
            GymViewModel gym = new GymViewModel();
            var gymString = await _cache.GetStringAsync(id.ToString());
            if (gymString != null)
            {
                gym = JsonSerializer.Deserialize<GymViewModel>(gymString);
                message = "Gym from CACHE: ";
            }

            if (gymString == null)
            {
                var gymModel = _gymService.GetGymModel(id);
                if (gymModel != null)
                {
                    message = "Gym from DB: ";
                    gym = _mapper.MapGymModelToViewModel(gymModel);
                    gymString = JsonSerializer.Serialize(gym);
                    await _cache.SetStringAsync(gym.Id.ToString(), gymString, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                }
                else
                {
                    message = "Gym from DB is not found: ";
                    gymString = string.Empty;
                }
            }
            
            return View("GetGym", message + gymString);
        }
    }
}
