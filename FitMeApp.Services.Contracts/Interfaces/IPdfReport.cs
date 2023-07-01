using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Services.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IPdfReport
    {
        void CreateDietPlanPdf(DietPdfReportModel dietPlan);
    }
}
