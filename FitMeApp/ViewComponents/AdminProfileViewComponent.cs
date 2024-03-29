﻿using FitMeApp.Common;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{
    public class AdminProfileViewComponent : ViewComponent
    {
        private readonly ITrainerService _trainerService;

        public AdminProfileViewComponent(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }


        public IViewComponentResult Invoke()
        {
            int trainerApplicationsCount = _trainerService.GetTrainerApplicationsCount();
            ViewBag.TrainerAppCount = trainerApplicationsCount;
            return View("AdminProfile");
        }
    }
}
