using FitMeApp.Mapper;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.ViewComponents
{

    public class TrainerWorkHoursViewComponent: ViewComponent
    {
        private readonly ITrainerService _trainerService;
        private readonly ModelViewModelMapper _mapper;
        public TrainerWorkHoursViewComponent(ITrainerService trainerService)
        {
            _trainerService = trainerService;
            _mapper = new ModelViewModelMapper();
        }

        public IViewComponentResult Invoke(string trainerId)
        {
            var trainerWorkHoursModels = _trainerService.GetWorkHoursByTrainer(trainerId);
            List<TrainerWorkHoursViewModel> trainerWorkHoursViewModels = new List<TrainerWorkHoursViewModel>();
            foreach (var model in trainerWorkHoursModels)
            {
                trainerWorkHoursViewModels.Add(_mapper.MapTrainerWorkHoursModelToViewModel(model));
            }
            return View("TrainerWorkHours", trainerWorkHoursViewModels);
        }



    }
}
