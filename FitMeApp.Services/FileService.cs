using System;
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
        private readonly IExcelService _excelService;
        private readonly ITxtService _txtService;

        public FileService(IExcelService excelService, ITxtService txtService)
        {
            _excelService = excelService;
            _txtService = txtService;
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
            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(fileStream);
            }
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
            var text = await _txtService.GetTextContentFromFileAsync(localPath);
            return text;
        }


        public string GetSpecifiedSectionFromFile(string localPath, string sectionStartMarker,
            string sectionEndMarker)
        {
            var sectionText = _txtService.GetSpecifiedSectionFromFile(localPath, sectionStartMarker, sectionEndMarker);
            return sectionText;
        }


        public IEnumerable<string> SplitTextIntoParagraphs(string text, string splitMark) //todo replace ro Common
        {
            var paragraphs = _txtService.SplitTextIntoParagraphs(text, splitMark);
            return paragraphs;
        }
        


        public async Task<List<AttendanceChartModel>> ReadAttendanceChartFromExcelAsync(byte[] buffer, int gymId)   //AttendanceChartModel only
        {
            List<AttendanceChartModel> output = await _excelService.ReadAttendanceChartFromExcelAsync(buffer);
            return output;
        }


        public async Task<NutrientsModel> ReadNutrientsFromExcelAsync(FileInfo file)
        {
            var nutrients = await _excelService.ReadNutrientsFromExcelAsync(file);
            return nutrients;
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
