﻿using FitMeApp.Services.Contracts.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace FitMeApp.Services
{
    public class ReportService: IReportService
    {
        private readonly IExcelReport _excelReport;

        public ReportService(IExcelReport excelReport)
        {
            _excelReport = excelReport;
        }



        public void WriteToExcel(DataTable table, FileInfo file)
        {
            string tableName = file.Name;
            tableName.Replace(file.Extension, string.Empty, true, CultureInfo.CurrentCulture);
            _excelReport.WriteToExcel(table, file, tableName); //EPPlus or OpenXml realization
        }
    }
}
