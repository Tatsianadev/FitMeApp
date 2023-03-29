﻿using FitMeApp.Services.Contracts.Models.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IExcelReport
    {
        void WriteToExcel(DataTable table, FileInfo file, string tableName);
        Task<List<TimeVisitorsModel>> ReadFromExcel(FileInfo file);
    }
}
