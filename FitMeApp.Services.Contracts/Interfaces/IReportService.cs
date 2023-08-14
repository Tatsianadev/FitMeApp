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
        Task CreateUsersListReportAsync(DataTable table, string fullPath);

        //Pdf
        void CreateDietPlanPdf(DietPdfReportModel dietPlan);

    }
}
