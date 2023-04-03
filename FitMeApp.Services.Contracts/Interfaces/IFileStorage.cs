using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FitMeApp.Services.Contracts.Models.Chart;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFileStorage
    {
        void SaveImageFileAsync(IFormFile uploadedFile,string path);
        void AddVisitingChartDataForGym(IEnumerable<VisitingChartModel> data);
    }
}
