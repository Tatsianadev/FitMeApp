using FitMeApp.Services.Contracts.Models;
using FitMeApp.Services.Contracts.Models.Chart;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IExcelService
    {
        Task CreateUsersListExcelFileAsync(DataTable table, FileInfo file, string tableName);
        Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(byte[] buffer);

        Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file);
    }
}
