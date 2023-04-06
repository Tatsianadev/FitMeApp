using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models.Chart;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFileService
    {
        string SetUniqueFileName();
        Task SaveFileAsync(IFormFile uploadedFile, string fullPath);
        Task<string> GetTextContentFromFileAsync(string localPath);
        void CopyFileToDirectory(string sourceFullPath, string destFullPath);
        Task WriteToExcelAsync(DataTable table, string fullPath);
        Task AddVisitingChartDataFromExcelToDbAsync(string fullPath, int gymId);

    }
}
