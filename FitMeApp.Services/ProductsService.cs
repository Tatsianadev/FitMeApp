using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Interfaces;
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
    }
}
