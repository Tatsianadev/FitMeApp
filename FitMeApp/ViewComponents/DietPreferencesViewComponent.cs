using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.WEB.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FitMeApp.ViewComponents
{
    public class DietPreferencesViewComponent: ViewComponent
    {
        private readonly IFileService _fileService;

        public DietPreferencesViewComponent(IFileService fileService)
        {
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FileInfo file = new FileInfo(Resources.Resources.NutrientsTablePath);
            var nutrientsModel = await _fileService.ReadNutrientsFromExcelAsync(file);

            var dietPreferences = new DietPreferencesViewModel()
            {
                AllNutrients = nutrientsModel,
                LovedNutrients = new NutrientsModel(),
                UnlovedNutrients = new NutrientsModel(),
                AllergicTo = new List<string>(),
                Budget = 0
            };

            return await Task.FromResult(View("DietPreferences", dietPreferences));
        }

    }
}
