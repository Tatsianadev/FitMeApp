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
                gyms.Add(_mapper.MappGymModelToViewModel(gym));
            }

            var groupClassModels = _fitMeService.GetAllGroupClassModels();
            List<GroupClassViewModel> groupClasses = new List<GroupClassViewModel>();
            foreach (var groupClass in groupClassModels)
            {
                groupClasses.Add(_mapper.MappGroupClassModelToViewModel(groupClass));
            }           
            ViewBag.GroupClasses = groupClasses; 
            
            return View(gyms);
        }


        [HttpPost]
        public ActionResult Index(List<int> SelectedGroupClassesId)
        {
            try
            {
                var selectedGymModels = _fitMeService.GetGymsOfGroupClasses(SelectedGroupClassesId);
                List<GymViewModel> selectedGyms = new List<GymViewModel>();
                foreach (var selectedGymModel in selectedGymModels)
                {
                    selectedGyms.Add(_mapper.MappGymModelToViewModel(selectedGymModel));
                }

                var groupClassModels = _fitMeService.GetAllGroupClassModels();
                List<GroupClassViewModel> groupClasses = new List<GroupClassViewModel>();
                foreach (var groupClass in groupClassModels)
                {
                    groupClasses.Add(_mapper.MappGroupClassModelToViewModel(groupClass));
                }
                ViewBag.GroupClasses = groupClasses;

                return View(selectedGyms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message); //show friendly Error Page!!
                throw ex;
            }            
        }

        public IActionResult CurrentGymInfo(int gymId)
        {
            var gymModel = _fitMeService.GetGymModel(gymId);
            GymViewModel gym = _mapper.MappGymModelToViewModel(gymModel);
            return View(gym);
        }

    }
}
