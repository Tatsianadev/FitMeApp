using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models.Chart;
using IronPython.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Scripting.Hosting;

namespace FitMeApp.Services
{
    public class ProductsService: IProductsService
    {
        private readonly ILogger<ProductsService> _logger;
        public ProductsService(ILogger<ProductsService> logger)
        {
            _logger = logger;
        }


        public IEnumerable<string> GetAllProductNames(string pythonFile) 
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            try
            {
                engine.ExecuteFile(pythonFile, scope);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
            
            var function = scope.GetVariable("getAllProductNames");
            string allProductsFile = scope.GetVariable("path");
            var productNamesPy = function(allProductsFile);
            var productNames = new List<string>();

            foreach (var name in productNamesPy)
            {
                productNames.Add(name);
            }

            return productNames;
        }


        public ProductNutrientsModel GetProductInfoByName(string pythonFile, string productName)
        {
            ScriptEngine engine = Python.CreateEngine();
            ScriptScope scope = engine.CreateScope();
            try
            {
                engine.ExecuteFile(pythonFile, scope);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }

            var function = scope.GetVariable("findProduct");
            string allProductsFile = scope.GetVariable("path");
            var productInfoPy = function(allProductsFile, productName);
            string calorie = productInfoPy["cal"];
            string protein = productInfoPy["prot"];
            string fat = productInfoPy["fat"];
            string carbohydrates = productInfoPy["carb"];
            var productNutrientsModel = new ProductNutrientsModel()
            {
                Name = productInfoPy["name"],
                Calorie = int.Parse(calorie.Replace(",", ".")),
                Protein = decimal.Parse(protein.Replace(",", "."), CultureInfo.InvariantCulture),
                Fat = decimal.Parse(fat.Replace(",", "."), CultureInfo.InvariantCulture),
                Carbohydrates = decimal.Parse(carbohydrates.Replace(",", "."), CultureInfo.InvariantCulture)
            };
           
            return productNutrientsModel;

        }
    }
}
