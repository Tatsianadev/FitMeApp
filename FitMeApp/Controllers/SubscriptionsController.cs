﻿using FitMeApp.Common;
using FitMeApp.Mapper;
using FitMeApp.Resources;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Controllers
{
    public class SubscriptionsController : Controller
    {
        private readonly IGymService _gymService;
        private readonly ITrainerService _trainerService;
        private readonly IScheduleService _scheduleService;
        private readonly ModelViewModelMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public SubscriptionsController(IGymService gymService,
            ITrainerService trainerService,
            IScheduleService scheduleService,
            UserManager<User> userManager,
            ILogger<GymsController> logger,
            IStringLocalizer<SharedResource> localizer)
        {
            _gymService = gymService;
            _trainerService = trainerService;
            _scheduleService = scheduleService;
            _mapper = new ModelViewModelMapper();
            _userManager = userManager;
            _logger = logger;
            _localizer = localizer;
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
                subscription.Image = GetImageSubscriptionPath(subscription);
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
                        subscription.Image = GetImageSubscriptionPath(subscription);
                    }

                    ViewBag.SubscriptionValidPeriods = _gymService.GetAllSubscriptionPeriods();
                    return View(subscriptions);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "There was a problem with using filters. Try again or not use filters, please.";

                return View("CustomError", message);
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
                    subscription.Image = GetImageSubscriptionPath(subscription);
                }
                return View(subscriptionViewModels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                string message = "Failed to display subscriptions for trainers. Please, try again";

                return View("CustomError", message);
            }

        }

        
        public async Task<IActionResult> CurrentTrainerSubscription(int subscriptionId, int gymId)
        {
            var user = await _userManager.GetUserAsync(User);
            var licenseModel = _trainerService.GetTrainerWorkLicenseByTrainer(user.Id);
            if (licenseModel != null && licenseModel.StartDate <= DateTime.Today && licenseModel.EndDate > DateTime.Today)
            {
                ViewBag.SubscriptionId = subscriptionId;
                return View("LicenseAlreadyExistsMessage", gymId);

            }
            else
            {
                var applicationForTrainerRole = _trainerService.GetTrainerApplicationByUser(user.Id);
                if (applicationForTrainerRole != null)
                {
                    return View("TrainerRoleAppIsPendingMessage", gymId);
                }
            }

            return RedirectToAction("CurrentSubscription", new { subscriptionId = subscriptionId, gymId = gymId });
        }


        public IActionResult TrainerRoleAppIsPendingMessage(int gymId)
        {
            return View(gymId);
        }


        public IActionResult LicenseAlreadyExistsMessage(int newGymId)
        {
            return View(newGymId);
        }


        public IActionResult CheckPossibilityToReplaceCurrentLicense(int subscriptionId, int newGymId)
        {
            var userId = _userManager.GetUserId(User);
            var licenseModel = _trainerService.GetTrainerWorkLicenseByTrainer(userId);

            if (licenseModel.GymId != newGymId)
            {
                var actualEventsCount = _scheduleService.GetActualEventsCountByTrainer(userId);
                if (actualEventsCount != 0)
                {
                    return View("FailedTryBuyTrainerSubscription");
                }
                else
                {
                    //delete trainers work hours in the previous gym. 
                    _trainerService.DeleteTrainerWorkHoursByTrainer(userId);
                }
            }

            return RedirectToAction("CurrentSubscription", new { subscriptionId = subscriptionId, gymId = newGymId });
        }


        public IActionResult FailedTryBuyTrainerSubscription()
        {
            return View();
        }

        
        public IActionResult CurrentSubscription(int subscriptionId, int gymId)
        {
            var subscriptionModel = _gymService.GetSubscriptionByGym(subscriptionId, gymId);
            SubscriptionViewModel subscription = _mapper.MapSubscriptionModelToViewModel(subscriptionModel);
            subscription.Image = GetImageSubscriptionPath(subscription);

            return View(subscription);
        }


        [HttpPost]
        [Authorize]
        public IActionResult CurrentSubscription(int gymId, int subscriptionId, DateTime startDate, bool isTrainerSubscription = false)
        {
            if (startDate.Date >= DateTime.Now.Date && startDate.Date <= DateTime.Now.AddDays(256).Date)
            {
                try
                {
                    string userId = _userManager.GetUserId(User);
                    int userSubscriptionId = _gymService.AddUserSubscription(userId, gymId, subscriptionId, startDate);
                    if (userSubscriptionId > 0)
                    {
                        if (isTrainerSubscription)
                        {
                            var licenseModel = _trainerService.GetTrainerWorkLicenseByTrainer(userId);
                            if (licenseModel != null)
                            {
                                //delete previous trainer subscription 
                                _gymService.DeleteUserSubscription(licenseModel.SubscriptionId);

                                var endDate = _gymService.GetUserSubscriptions(userId)
                                    .FirstOrDefault(x => x.Id == userSubscriptionId).EndDate;

                                var newLicense = new TrainerWorkLicenseModel()
                                {
                                    TrainerId = userId,
                                    SubscriptionId = userSubscriptionId,
                                    GymId = gymId,
                                    StartDate = startDate,
                                    EndDate = endDate,
                                    ConfirmationDate = DateTime.Today
                                };

                                _trainerService.ReplaceTrainerWorkLicense(userId, newLicense);
                            }
                            else
                            {
                                AddTrainerApplication(userId);
                            }
                        }
                        return View("SubscriptionCompleted");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                    string message = "There was a problem with to subscribe. Please, try again";

                    return View("CustomError", message);
                }
            }

            return RedirectToAction("CurrentSubscription", new { subscriptionId = subscriptionId, gymId = gymId });
        }





        private string GetImageSubscriptionPath(SubscriptionViewModel subscription)
        {
            string imagePath;

            if (subscription.WorkAsTrainer)
            {
                imagePath = _localizer["WorkAsTrainer"];
            }
            else if (subscription.DietMonitoring && subscription.GroupTraining == false)
            {
                imagePath = _localizer["GymAccessDietMonitoring"];
            }
            else if (subscription.DietMonitoring == false && subscription.GroupTraining)
            {
                imagePath = _localizer["GymAccessGroupTraining"];
            }
            else
            {
                imagePath = _localizer["GymAccessGroupTrainingDietMonitoring"];
            }

            return imagePath;
        }


        private void AddTrainerApplication(string userId)
        {
            TrainerApplicationModel application = new TrainerApplicationModel()
            {
                UserId = userId,
                TrainerSubscription = true,
                ApplicationDate = DateTime.Today
            };

            _trainerService.AddTrainerApplication(application);
        }
    }
}
