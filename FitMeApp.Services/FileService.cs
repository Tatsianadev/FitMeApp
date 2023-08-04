﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Models;
using FitMeApp.Services.Contracts.Models.Chart;
using Microsoft.AspNetCore.Http;


namespace FitMeApp.Services
{
    public sealed class FileService : IFileService
    {
        private readonly IFileStorage _fileStorage;
        private readonly IReportService _reportService;

        public FileService(IFileStorage fileStorage, IReportService reportServiceService)
        {
            _fileStorage = fileStorage;
            _reportService = reportServiceService;
        }


        public string SetUniqueFileName()
        {
            var fileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            fileName = Regex.Replace(fileName, "[^a-zA-Z0-9]", "");
            return fileName;
        }


        public async Task SaveFileAsync(IFormFile uploadedFile, string fullPath)
        {
            string directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            DeleteFileIfExist(fullPath);
            await _fileStorage.SaveFileAsync(uploadedFile, fullPath);
        }


        public void CopyFileToDirectory(string sourceFullPath, string destFullPath)
        {
            if (File.Exists(sourceFullPath))
            {
                if (!Directory.Exists(Path.GetDirectoryName(destFullPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destFullPath));
                }

                File.Copy(sourceFullPath, destFullPath, true);
            }
        }

        
        //Text
        public async Task<string> GetTextContentFromFileAsync(string localPath)
        {
            var text = await _reportService.GetTextContentFromFileAsync(localPath);
            return text;
        }


        public string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker,
            string sectionEndMarker)
        {
            var sectionText = _reportService.GetSpecifiedSectionFromFile(localPath, sectionStartMarker, sectionEndMarker);
            return sectionText;
        }


        public IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark)
        {
            var paragraphs = _reportService.SplitTextIntoParagraphs(text, splitMark);
            return paragraphs;
        }
        
       

        //Excel
        public async Task CreateUsersListExcelFileAsync(DataTable table, string fullPath)
        {
            FileInfo file = new FileInfo(fullPath);
            DeleteFileIfExist(fullPath);
            await _reportService.CreateUsersListExcelFileAsync(table, file); //EPPlus or OpenXml realization
        }


        public async Task AddVisitingChartDataFromExcelToDbAsync(byte[] buffer, int gymId)   //AttendanceChartModel only
        {
            List<AttendanceChartModel> output = await ReadAttendanceChartFromExcelAsync(buffer);
            output = output.OrderBy(x => x.DayOfWeek).ToList();
            foreach (var model in output)
            {
                model.GymId = gymId;
                model.NumberOfVisitorsPerHour = model.NumberOfVisitorsPerHour.OrderBy(x => x.Hour).ToList();
            }

            _fileStorage.AddVisitingChartDataToDb(output);
        }


        public async Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file)
        {
            var nutrients = await _reportService.ReadNutrientsFromExcelAsync(file);
            return nutrients;
        }

        
        //Pdf
        public void CreateDietPlanPdf(DietPdfReportModel dietPlan)
        {
            _reportService.CreateDietPlanPdf(dietPlan);
        }


        private async Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(byte[] buffer)  //AttendanceChartModel only
        {
            //FileInfo file = new FileInfo(fullPath);
            List<AttendanceChartModel> output = await _reportService.ReadAttendanceChartFromExcelAsync(buffer);
            return output;
        }


        private void DeleteFileIfExist(string fullPath)
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

    }
}
