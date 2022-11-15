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
                gyms.Add(_mapper.MappGymModelToViewModelBase(gym));
            }

            var trainingModels = _fitMeService.GetAllTrainingModels(); //info for filter by trainings
            List<TrainingViewModel> trainings = new List<TrainingViewModel>();
            foreach (var training in trainingModels)
            {
                trainings.Add(_mapper.MappTrainingModelToViewModelBase(training));
            }
            ViewBag.Trainings = trainings;

            return View(gyms);
        }


        [HttpPost]
        public ActionResult Index(List<int> SelectedTrainingsId)
        {
            try
            {
                var selectedGymModels = _fitMeService.GetGymsByTrainings(SelectedTrainingsId);
                List<GymViewModel> selectedGyms = new List<GymViewModel>();
                foreach (var selectedGymModel in selectedGymModels)
                {
                    selectedGyms.Add(_mapper.MappGymModelToViewModelBase(selectedGymModel));
                }

                var trainingModels = _fitMeService.GetAllTrainingModels(); //info for filter by trainings
                List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                foreach (var training in trainingModels)
                {
                    trainings.Add(_mapper.MappTrainingModelToViewModelBase(training));
                }
                ViewBag.Trainings = trainings;

                return View(selectedGyms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with using filters. Try again or not use filters, please."
                };
                return View("CustomError", error);
            }            
        }

        public IActionResult CurrentGymInfo(int gymId)
        {
            var gymModel = _fitMeService.GetGymModel(gymId);
            GymViewModel gym = _mapper.MappGymModelToViewModel(gymModel);
            List<TrainingViewModel> trainings = new List<TrainingViewModel>();
            List<int> trainingsId = new List<int>();
            foreach (var trainer in gym.Trainers)
            {
                foreach (var training in trainer.Trainings)
                {
                    if (!trainingsId.Contains(training.Id)&&training.Name != "Personal training")
                    {
                        trainings.Add(training);
                        trainingsId.Add(training.Id);
                    }                   
                }                
            }
            
            ViewBag.Trainings = trainings;
            return View(gym);
        }


       
        public ActionResult Subscriptions(int gymId)
        {
            List<SubscriptionViewModel> subscriptions = new List<SubscriptionViewModel>();           
            var subscriptionModels = _fitMeService.GetSubscriptionsByGym(gymId);

            foreach (var subscriptionModel in subscriptionModels)
            {
                subscriptions.Add(_mapper.MappSubscriptionModelToViewModel(subscriptionModel));                
            }

            foreach (var subscription in subscriptions)
            {
                subscription.Image = (subscription.GroupTraining ? nameof(subscription.GroupTraining) : "") 
                    + (subscription.DietMonitoring ? nameof(subscription.DietMonitoring) : ""); 
            }

            ViewBag.SubscriptionValidPeriods = _fitMeService.GetAllSubscriptionPeriods();
            ViewBag.GymId = gymId;
            return View(subscriptions);
        }




        [HttpPost]
        public ActionResult Subscriptions(int gymId, List<int> selectedPeriods, bool groupTraining, bool dietMonitoring)
        {
            try
            {
                List<SubscriptionViewModel> subscriptions = new List<SubscriptionViewModel>();
                var subscriptioModels = _fitMeService.GetSubscriptionsByGymByFilter(gymId, selectedPeriods, groupTraining, dietMonitoring);
                foreach (var subscriptionModel in subscriptioModels)
                {
                    subscriptions.Add(_mapper.MappSubscriptionModelToViewModel(subscriptionModel));
                }

                foreach (var subscription in subscriptions)
                {
                    subscription.Image = (subscription.GroupTraining ? nameof(subscription.GroupTraining) : "")
                        + (subscription.DietMonitoring ? nameof(subscription.DietMonitoring) : "");
                }

                ViewBag.SubscriptionValidPeriods = _fitMeService.GetAllSubscriptionPeriods();
                return View(subscriptions);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "There was a problem with using filters. Try again or not use filters, please."
                };
                return View("CustomError", error);
            }
            
        }
    }
}
