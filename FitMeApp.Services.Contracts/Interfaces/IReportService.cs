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
        Task WriteToExcelAsync(DataTable table, FileInfo file);
        Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(FileInfo file);

        Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file);

        //Pdf
        void CreateDietPlanPdf(DietPdfReportModel dietPlan);

    }
}
