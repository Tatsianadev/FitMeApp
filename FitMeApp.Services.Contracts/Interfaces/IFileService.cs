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
        string SaveAvatarFileAsync(string userId, IFormFile uploadedFile, string rootPath);
        Task<string> GetTextContentFromFile(string localPath);
        void CopyFileToDirectory(string sourceFileName, string destFileName);
        void WriteToExcel(DataTable table, string fullPath);
        void SaveFromExcelToDb(string fullPath, int gymId);
        //Task<List<VisitingChartModel>> ReadFromExcel(string fullPath);
        
    }
}
