using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private ILogger _logger;

        public GymsController(IFitMeService fitMeService, UserManager<User> userManager, ILoggerFactory loggerFactory)
        {
            _fitMeService = fitMeService;
            _mapper = new ModelViewModelMapper();
            _userManager = userManager;
            _logger = loggerFactory.CreateLogger("GymsControllerLogger");
            
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
                if (SelectedTrainingsId.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                else
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
                    if (!trainingsId.Contains(training.Id) && training.Name != "Personal training")
                    {
                        trainings.Add(training);
                        trainingsId.Add(training.Id);
                    }
                }
            }

            ViewBag.Trainings = trainings;
            ViewBag.WorkHours = _fitMeService.GetWorkHoursByGym(gymId);
            return View(gym);
        }



        public ActionResult Subscriptions(int gymId)
        {
            List<SubscriptionViewModel> subscriptions = new List<SubscriptionViewModel>();
            var subscriptionModels = _fitMeService.GetAllSubscriptionsByGym(gymId);

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
                if (selectedPeriods.Count == 0 && groupTraining == false && dietMonitoring == false)
                {
                    return RedirectToAction("Subscriptions", new { gymId = gymId });
                }
                else
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

        public ActionResult CurrentSubscription(int subscriptionId, int gymId)
        {
            var subscriptionModel = _fitMeService.GetSubscriptionByGym(subscriptionId, gymId);
            SubscriptionViewModel subscription = _mapper.MappSubscriptionModelToViewModel(subscriptionModel);

            subscription.Image = (subscription.GroupTraining ? nameof(subscription.GroupTraining) : "")
                + (subscription.DietMonitoring ? nameof(subscription.DietMonitoring) : "");

            return View(subscription);
        }


        [HttpPost]
        [Authorize(Roles ="admin, user")]
    
        public ActionResult CurrentSubscription(int gymId, int subscriptionId, DateTime startDate)
        {
            if (startDate.Date >= DateTime.Now.Date && startDate.Date<=DateTime.Now.AddDays(256).Date)
            {
                try
                {
                    string userId = _userManager.GetUserId(User);
                    bool result = _fitMeService.AddUserSubscription(userId, gymId, subscriptionId, startDate);
                    return View("SubscriptionCompleted");
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
            }

            return View();
        }
    }
}
