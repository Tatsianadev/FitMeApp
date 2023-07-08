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
        

        public async Task<string> GetTextContentFromFileAsync(string localPath)
        {
            FileInfo fileInfo = new FileInfo(localPath);
            string pathToTextMessage = fileInfo.FullName;
            string text = string.Empty;

            using (FileStream fstream = new FileStream(pathToTextMessage, FileMode.Open))
            {
                byte[] buffer = new byte[fstream.Length];
                await fstream.ReadAsync(buffer, 0, buffer.Length);
                text = Encoding.Default.GetString(buffer);
            }

            return text;
        }


        public string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker,
            string sectionEndMarker)
        {
            bool isInSection = false;
            var sectionContent = new StringBuilder();
            using (StreamReader reader = new StreamReader(localPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains(sectionStartMarker, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isInSection = true;
                        continue;
                    }

                    if (line.Contains(sectionEndMarker, StringComparison.CurrentCultureIgnoreCase))
                    {
                        isInSection = false;
                        break;
                    }

                    if (isInSection)
                    {
                        sectionContent.AppendLine(line);
                    }
                }
            }

            string sectionText = sectionContent.ToString();
            
            return sectionText;
        }


        public IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark)
        {
            var paragraphs = new List<string>();
            if (!string.IsNullOrEmpty(text))
            {
                paragraphs = Regex.Split(text, splitMark).Where(p => p.Any(char.IsLetterOrDigit)).ToList();
            }

            return paragraphs;
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


        public async Task WriteToExcelAsync(DataTable table, string fullPath)
        {
            FileInfo file = new FileInfo(fullPath);
            DeleteFileIfExist(fullPath);
            await _reportService.WriteToExcelAsync(table, file); //EPPlus or OpenXml realization
        }


        public async Task AddVisitingChartDataFromExcelToDbAsync(string fullPath, int gymId)   //AttendanceChartModel only
        {
            List<AttendanceChartModel> output = await ReadAttendanceChartFromExcelAsync(fullPath);
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


        public void CreateDietPlanPdf(DietPdfReportModel dietPlan)
        {
            _reportService.CreateDietPlanPdf(dietPlan);
        }


        private async Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(string fullPath)  //AttendanceChartModel only
        {
            FileInfo file = new FileInfo(fullPath);
            List<AttendanceChartModel> output = await _reportService.ReadAttendanceChartFromExcelAsync(file);
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
