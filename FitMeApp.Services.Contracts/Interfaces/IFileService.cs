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
        Task<string> GetTextContentFromFileAsync(string localPath); //todo remove txt report

        string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker, string sectionEndMarker); //todo remove txt report
        IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark); //todo remove txt report
        void CopyFileToDirectory(string sourceFullPath, string destFullPath);

        //Excel
        Task WriteToExcelAsync(DataTable table, string fullPath);
        Task AddVisitingChartDataFromExcelToDbAsync(string fullPath, int gymId);
        Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file);

        //Pdf
        void CreateDietPlanPdf(DietPdfReportModel dietPlan);

    }
}
