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
    public sealed class ReportService : IReportService
    {
        private readonly IExcelService _excelService;
        private readonly IPdfService _pdfService;

        public ReportService(IExcelService excelService, IPdfService pdfService)
        {
            _excelService = excelService;
            _pdfService = pdfService;
        }

        
        public async Task CreateUsersListReportAsync(DataTable table, string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            FileInfo file = new FileInfo(fullPath);
            string tableName = file.Name;
            tableName.Replace(file.Extension, string.Empty, true, CultureInfo.CurrentCulture);
            await _excelService.CreateUsersListExcelFileAsync(table, file, tableName); //EPPlus or OpenXml realization
        }

        
        public void CreateDietPlanPdf(DietPdfReportModel dietPlan)
        {
            _pdfService.CreateDietPlanPdf(dietPlan);
        }
    }
}
