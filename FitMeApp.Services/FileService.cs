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
using FitMeApp.Services.Contracts.Models.Chart;
using Microsoft.AspNetCore.Http;


namespace FitMeApp.Services
{
    public class FileService : IFileService
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


        public void WriteToExcel(DataTable table, string fullPath)
        {
            FileInfo file = new FileInfo(fullPath);
            DeleteFileIfExist(fullPath);
            _reportService.WriteToExcel(table, file); //EPPlus or OpenXml realization
        }


        public async Task AddVisitingChartDataFromExcelToDbAsync(string fullPath, int gymId)   //VisitingChartModel only
        {
            List<VisitingChartModel> output = await ReadFromExcelAsync(fullPath);
            output = output.OrderBy(x => x.DayOfWeek).ToList();
            foreach (var model in output)
            {
                model.GymId = gymId;
                model.TimeVisitorsLine = model.TimeVisitorsLine.OrderBy(x => x.Hour).ToList();
            }
            
            _fileStorage.AddVisitingChartDataToDb(output);
        }




        private async Task<List<VisitingChartModel>> ReadFromExcelAsync(string fullPath)  //VisitingChartModel only
        {
            FileInfo file = new FileInfo(fullPath);
            List<VisitingChartModel> output = await _reportService.ReadFromExcel(file);
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
