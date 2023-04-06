using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FitMeApp.Services.Contracts.Models.Chart;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFileStorage
    {
        Task SaveFileAsync(IFormFile uploadedFile,string fullPath);
        void AddVisitingChartDataToDb(IEnumerable<VisitingChartModel> data);
    }
}
