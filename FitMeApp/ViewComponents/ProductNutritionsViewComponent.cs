using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
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
            string pythonFile = Environment.CurrentDirectory + Resources.Resources.DietJournalPyPath;
            var product = _dietService.GetProductInfoByName(pythonFile, productName);
            var productViewModel = new ProductNutrientsViewModel()
            {
                Name = product.Name,
                Calorie = product.Calorie,
                Protein = product.Protein,
                Fat = product.Fat,
                Carbohydrates = product.Carbohydrates
            };
            return View("ProductNutrients", productViewModel);
        }
    }
}
