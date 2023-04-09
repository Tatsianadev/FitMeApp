using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.Models.ExcelModels
{
    public class AttendanceChartExcelFileViewModel
    {
        public int GymId { get; set; }
        public string GymName { get; set; }
        public string BlankFullPath { get; set; }
        public IFormFile AttendanceChartFile { get; set; }
        
    }
}
