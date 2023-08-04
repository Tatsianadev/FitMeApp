using FitMeApp.Services.Contracts.Models.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IReportService
    {
        //Excel
        Task CreateUsersListExcelFileAsync(DataTable table, FileInfo file);
        Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(byte[] buffer);
        Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file);

        //Pdf
        void CreateDietPlanPdf(DietPdfReportModel dietPlan);

        //Text
        Task<string> GetTextContentFromFileAsync(string localPath);
        string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker, string sectionEndMarker);
        IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark);
    }
}
