using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FitMeApp.ViewComponents
{
    public class ProductNutrientsViewComponent: ViewComponent
    {
        private readonly IDietService _dietService;
        public ProductNutrientsViewComponent(IDietService dietService)
        {
            _dietService = dietService;
        }

        public IViewComponentResult Invoke(string productName)
        {

            return View("ProductNutrients");
        }
    }
}
