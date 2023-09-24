using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models.Chart;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace FitMeApp.ViewComponents
{
    public class ProductNutrientsViewComponent: ViewComponent
    {
        private readonly ICacheService _cache;
        private readonly IDietService _dietService;
        public ProductNutrientsViewComponent(IDietService dietService, ICacheService cache)
        {
            _dietService = dietService;
            _cache = cache;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productName)
        {
            var product = await _cache.GetValueAsync<ProductNutrientsModel>(productName);
            if (product == null)
            {
                string pythonFile = Environment.CurrentDirectory + Resources.Resources.DietJournalPyPath;
                product = _dietService.GetProductInfoByName(pythonFile, productName);
                await _cache.SetValueAsync(productName, product, TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));
            }
            
            var productViewModel = new ProductNutrientsViewModel()
            {
                Name = product.Name.ToUpper(),
                Calorie = product.Calorie,
                Protein = product.Protein,
                Fat = product.Fat,
                Carbohydrates = product.Carbohydrates
            };
            return await Task.FromResult(View("ProductNutrients", productViewModel));
        }
    }
}
