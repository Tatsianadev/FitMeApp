using FitMeApp.Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using FitMeApp.Services.Contracts.Models.Chart;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services
{
    public sealed class ReportService: IReportService
    {
        private readonly IExcelReport _excelReport;
        private readonly IPdfReport _pdfReport;

        public ReportService(IExcelReport excelReport, IPdfReport pdfReport)
        {
            _excelReport = excelReport;
            _pdfReport = pdfReport;
        }



        public async Task WriteToExcelAsync(DataTable table, FileInfo file)
        {
            string tableName = file.Name;
            tableName.Replace(file.Extension, string.Empty, true, CultureInfo.CurrentCulture);
            await _excelReport.WriteToExcelAsync(table, file, tableName); //EPPlus or OpenXml realization
        }


        public async Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(FileInfo file)
        {
            List<AttendanceChartModel> output = await _excelReport.ReadAttendanceChartFromExcelAsync(file);
            return output;
        }


        public async Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file)
        {
            var output = await _excelReport.ReadNutrientsFromExcelAsync(file);
            return output;
        }


        public void CreateDietPlanPdf(DietPdfReportModel dietPlan)
        {
            _pdfReport.CreateDietPlanPdf(dietPlan);
        }
    }
}
