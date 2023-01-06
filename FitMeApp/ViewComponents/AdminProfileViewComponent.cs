using FitMeApp.Common;
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
        private readonly UserManager<User> _userManager;
        private readonly IFitMeService _fitMeService;

        public AdminProfileViewComponent(UserManager<User> userManager, IFitMeService fitMeService)
        {
            _userManager = userManager;
            _fitMeService = fitMeService;
        }


        public IViewComponentResult Invoke()
        {
            var trainersInPending = _fitMeService.GetAllTrainerModels()
                                                  .Where(x => x.Status == Common.TrainerApproveStatusEnum.pending)
                                                  .ToList();
            ViewBag.TrainersInPendingCount = trainersInPending.Count;
                                                  
            return View("AdminProfile");
        }
    }
}
