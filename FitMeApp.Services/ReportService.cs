﻿using FitMeApp.Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using FitMeApp.Services.Contracts.Models.Chart;
using System.Threading.Tasks;

namespace FitMeApp.Services
{
    public class ReportService: IReportService
    {
        private readonly IExcelReport _excelReport;

        public ReportService(IExcelReport excelReport)
        {
            _excelReport = excelReport;
        }



        public async Task WriteToExcelAsync(DataTable table, FileInfo file)
        {
            string tableName = file.Name;
            tableName.Replace(file.Extension, string.Empty, true, CultureInfo.CurrentCulture);
            await _excelReport.WriteToExcelAsync(table, file, tableName); //EPPlus or OpenXml realization
        }

        public async Task<List<AttendanceChartModel>> ReadFromExcelAsync(FileInfo file)
        {
            List<AttendanceChartModel> output = await _excelReport.ReadFromExcelAsync(file);
            return output;
        }
    }
}
