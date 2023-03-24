using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IExcelReport
    {
        void WriteToExcel(DataTable table, FileInfo file, string tableName);
    }
}
