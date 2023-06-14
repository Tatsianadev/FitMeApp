using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FitMeApp.ViewComponents
{
    public class CurrentTrainerInfoViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke(TrainerViewModel trainer)
        {
            return View("CurrentTrainerInfo", trainer);
        }
    }
}
