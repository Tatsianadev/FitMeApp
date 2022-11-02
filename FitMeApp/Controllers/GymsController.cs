using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class GymsController : Controller
    {
        private readonly IFitMeService _fitMeService;
        private readonly ModelViewModelMapper _mapper;
        private ILogger _logger;

        public GymsController(IFitMeService fitMeService, ILoggerFactory loggerFactory )
        {
            _fitMeService = fitMeService;
            _logger = loggerFactory.CreateLogger("GymsControllerLogger");
            _mapper = new ModelViewModelMapper();
        }



        public IActionResult Index()
        {
            var gymModels = _fitMeService.GetAllGymModels();
            List<GymViewModel> gyms = new List<GymViewModel>();
            foreach (var gym in gymModels)
            {
                gyms.Add(_mapper.MappGymModelToModelView(gym));
            }

            var groupClassModels = _fitMeService.GetAllGroupClassModels();
            List<string> groupClassNames = new List<string>();
            foreach (var groupClass in groupClassModels)
            {
                groupClassNames.Add(groupClass.Name);
            }
            ViewBag.GroupClassNames = groupClassNames;

            return View(gyms);
        }

        [HttpPost]
        public ActionResult Index(List<string> Districts, List<string> SelectedGroupClassNames)
        {
            var allGymModels = _fitMeService.GetAllGymModels();           
            var selectedGyms = new List<GymViewModel>();

            foreach (var gym in allGymModels)
            {
                bool gymAdded = false;
                foreach (var CurrentGymGroupClass in gym.GroupClasses)
                {
                    foreach (var groupClassName in SelectedGroupClassNames)
                    {
                        if (CurrentGymGroupClass.Name == groupClassName && !gymAdded)
                        {
                            selectedGyms.Add(_mapper.MappGymModelToModelView(gym));
                            gymAdded = true;
                        }
                    }
                }
            }

            var allGroupClassModels = _fitMeService.GetAllGroupClassModels();
            List<string> groupClassNames = new List<string>();
            foreach (var groupClass in allGroupClassModels)
            {
                groupClassNames.Add(groupClass.Name);
            }
            ViewBag.GroupClassNames = groupClassNames;

            return View(selectedGyms);
        }

    }
}
