using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.WEB.Contracts.ViewModels;
using FitMeApp.WEB.Contracts.ViewModels.Chart;
using Microsoft.Extensions.Logging;

namespace FitMeApp.Controllers
{
    public class ChartController : Controller
    {
        private readonly IFileService _fileService;
        private readonly ILogger _logger;

        public ChartController(IFileService fileService, ILogger<ChartController> logger)
        {
            _fileService = fileService;
            _logger = logger;
        }


        public async Task<IActionResult> VisitingChart()
        {
            try
            {
                List<VisitingChartViewModel> visitingData = new List<VisitingChartViewModel>();
                string fullPath = Environment.CurrentDirectory + @"\wwwroot\ExcelFiles\Import\BigRock\VisitorsChart.xlsx";
                var output = await _fileService.ReadFromExcel(fullPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return View();
        }
    }
}
