using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IProductsService
    {
        IEnumerable<string> GetAllProductNames(string pythonFile);
    }
}
