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
        private readonly IGymService _gymService;
        private readonly ITrainingService _trainingService;
        private readonly ModelViewModelMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;

        public GymsController(IGymService gymService, ITrainingService trainingService, UserManager<User> userManager, ILogger<GymsController> logger)
        {
            _gymService = gymService;
            _trainingService = trainingService;
            _mapper = new ModelViewModelMapper();
            _userManager = userManager;
            _logger = logger;

        }

        
        public IActionResult Index()
        {
            var gymModels = _gymService.GetAllGymsWithGalleryModels();
            List<GymViewModel> gyms = new List<GymViewModel>();
            foreach (var gym in gymModels)
            {
                gyms.Add(_mapper.MapGymModelToViewModelBase(gym));
            }

            var trainingModels = _trainingService.GetAllTrainingModels(); //info for filter by trainings
            List<TrainingViewModel> trainings = new List<TrainingViewModel>();
            foreach (var training in trainingModels)
            {
                trainings.Add(_mapper.MapTrainingModelToViewModelBase(training));
            }
            ViewBag.Trainings = trainings;

            return View(gyms);
        }


        [HttpPost]
        public IActionResult Index(List<int> selectedTrainingsId)
        {
            try
            {
                if (selectedTrainingsId.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    var selectedGymModels = _gymService.GetGymsByTrainings(selectedTrainingsId);
                    List<GymViewModel> selectedGyms = new List<GymViewModel>();
                    foreach (var selectedGymModel in selectedGymModels)
                    {
                        selectedGyms.Add(_mapper.MapGymModelToViewModelBase(selectedGymModel));
                    }

                    var trainingModels = _trainingService.GetAllTrainingModels(); //info for filter by trainings
                    List<TrainingViewModel> trainings = new List<TrainingViewModel>();
                    foreach (var training in trainingModels)
                    {
                        trainings.Add(_mapper.MapTrainingModelToViewModelBase(training));
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
            var gymModel = _gymService.GetGymModel(gymId);
            GymViewModel gym = _mapper.MapGymModelToViewModel(gymModel);
            List<TrainingViewModel> trainings = new List<TrainingViewModel>();
            foreach (var trainer in gym.Trainers)
            {
                foreach (var training in trainer.Trainings)
                {
                    if (!trainings.Select(x => x.Id).ToList().Contains(training.Id) && training.Name != "Personal training")
                    {
                        trainings.Add(training);
                    }
                }
            }

            ViewBag.Trainings = trainings;
            ViewBag.WorkHours = _gymService.GetWorkHoursByGym(gymId);
            return View(gym);
        }


        public IActionResult Subscriptions(int gymId)
        {
            var gymModel = _gymService.GetGymModel(gymId);
            GymViewModel gymViewModel = _mapper.MapGymModelToViewModel(gymModel);
            return View(gymViewModel);
        }


        public IActionResult SubscriptionsForVisitors(int gymId)
        {
            List<SubscriptionViewModel> subscriptions = new List<SubscriptionViewModel>();
            var subscriptionModels = _gymService.GetAllSubscriptionsForVisitorsByGym(gymId);

            foreach (var subscriptionModel in subscriptionModels)
            {
                subscriptions.Add(_mapper.MapSubscriptionModelToViewModel(subscriptionModel));
            }

            foreach (var subscription in subscriptions)
            {
                subscription.Image = "gym" + (subscription.GroupTraining ? nameof(subscription.GroupTraining) : "")
                                           + (subscription.DietMonitoring ? nameof(subscription.DietMonitoring) : "");
            }

            ViewBag.SubscriptionValidPeriods = _gymService.GetAllSubscriptionPeriods();
            return View(subscriptions);
        }




        [HttpPost]
        public IActionResult SubscriptionsForVisitors(int gymId, List<int> selectedPeriods, bool groupTraining, bool dietMonitoring)
        {
            try
            {
                if (selectedPeriods.Count == 0 && groupTraining == false && dietMonitoring == false)
                {
                    return RedirectToAction("SubscriptionsForVisitors", new { gymId = gymId });
                }
                else
                {
                    if (selectedPeriods.Count == 0)
                    {
                        selectedPeriods = _gymService.GetAllSubscriptionPeriods().ToList();
                    }
                    List<SubscriptionViewModel> subscriptions = new List<SubscriptionViewModel>();
                    var subscriptionModels = _gymService.GetSubscriptionsForVisitorsByGymByFilter(gymId, selectedPeriods, groupTraining, dietMonitoring);
                    foreach (var subscriptionModel in subscriptionModels)
                    {
                        subscriptions.Add(_mapper.MapSubscriptionModelToViewModel(subscriptionModel));
                    }

                    foreach (var subscription in subscriptions)
                    {
                        subscription.Image = "gym" + (subscription.GroupTraining ? nameof(subscription.GroupTraining) : "")
                            + (subscription.DietMonitoring ? nameof(subscription.DietMonitoring) : "");
                    }

                    ViewBag.SubscriptionValidPeriods = _gymService.GetAllSubscriptionPeriods();
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


        public IActionResult SubscriptionsForTrainers(int gymId)
        {
            try
            {
                var subscriptionModels = _gymService.GetAllSubscriptionsForTrainersByGym(gymId);
                List<SubscriptionViewModel> subscriptionViewModels = new List<SubscriptionViewModel>();
                foreach (var subscriptionModel in subscriptionModels)
                {
                    subscriptionViewModels.Add(_mapper.MapSubscriptionModelToViewModel(subscriptionModel));
                }

                foreach (var subscription in subscriptionViewModels)
                {
                    subscription.Image = nameof(subscription.WorkAsTrainer);
                }
                return View(subscriptionViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                CustomErrorViewModel error = new CustomErrorViewModel()
                {
                    Message = "Failed to display subscriptions for trainers. Please, try again"
                };
                return View("CustomError", error);
            }

        }


        public IActionResult CurrentSubscription(int subscriptionId, int gymId)
        {
            var subscriptionModel = _gymService.GetSubscriptionByGym(subscriptionId, gymId);
            SubscriptionViewModel subscription = _mapper.MapSubscriptionModelToViewModel(subscriptionModel);

            if (subscription.WorkAsTrainer)
            {
                subscription.Image = nameof(subscription.WorkAsTrainer);
            }
            else
            {
                subscription.Image = "gym" + (subscription.GroupTraining ? nameof(subscription.GroupTraining) : "")
                                     + (subscription.DietMonitoring ? nameof(subscription.DietMonitoring) : "");
            }

            return View(subscription);
        }


        [HttpPost]
        [Authorize]

        public IActionResult CurrentSubscription(int gymId, int subscriptionId, DateTime startDate)
        {
            if (startDate.Date >= DateTime.Now.Date && startDate.Date <= DateTime.Now.AddDays(256).Date)
            {
                try
                {
                    string userId = _userManager.GetUserId(User);
                    bool subscriptionIsAdded = _gymService.AddUserSubscription(userId, gymId, subscriptionId, startDate);
                    if (subscriptionIsAdded)
                    {
                        return View("SubscriptionCompleted");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    CustomErrorViewModel error = new CustomErrorViewModel()
                    {
                        Message = "There was a problem with to subscribe. Please, try again"
                    };
                    return View("CustomError", error);
                }
            }

            return RedirectToAction("CurrentSubscription", new { subscriptionId = subscriptionId, gymId = gymId });
        }
    }
}
