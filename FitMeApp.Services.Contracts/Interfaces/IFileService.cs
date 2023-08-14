using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.Services.Contracts.Models.Chart;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IFileService
    {
        string SetUniqueFileName();
        Task SaveFileAsync(IFormFile uploadedFile, string fullPath);
        void CopyFileToDirectory(string sourceFullPath, string destFullPath);

        //Text
        Task<string> GetTextContentFromFileAsync(string localPath); 
        string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker, string sectionEndMarker);
        IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark);

        //Excel
        Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(byte[] buffer, int gymId);
        Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file);
    }
}
