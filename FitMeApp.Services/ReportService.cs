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
        private readonly ITextReport _textReport;

        public ReportService(IExcelReport excelReport, IPdfReport pdfReport, ITextReport textReport)
        {
            _excelReport = excelReport;
            _pdfReport = pdfReport;
            _textReport = textReport;
        }



        public async Task CreateUsersListExcelFileAsync(DataTable table, FileInfo file)
        {
            string tableName = file.Name;
            tableName.Replace(file.Extension, string.Empty, true, CultureInfo.CurrentCulture);
            await _excelReport.CreateUsersListExcelFileAsync(table, file, tableName); //EPPlus or OpenXml realization
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


        //Text
        public async Task<string> GetTextContentFromFileAsync(string localPath)
        {
            var text = await _textReport.GetTextContentFromFileAsync(localPath);
            return text;
        }


        public string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker, string sectionEndMarker)
        {
            var sectionText = _textReport.GetSpecifiedSectionFromFile(localPath, sectionStartMarker, sectionEndMarker);
            return sectionText;
        }


        public IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark)
        {
            var paragraphs = _textReport.SplitTextIntoParagraphs(text, splitMark);
            return paragraphs;
        }
    }
}
