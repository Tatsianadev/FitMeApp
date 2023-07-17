using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Models.Chart;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<string> GetAllProductNames(string pythonFile);
        ProductNutrientsModel GetProductInfoByName(string pythonFile, string productName);
    }
}
