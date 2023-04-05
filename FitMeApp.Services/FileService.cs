using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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


        //public string SaveAvatarFileAsync(string userId, IFormFile uploadedFile, string rootPath)
        //{
        //    string directoryPath = rootPath + "/Content/Upload/" + userId + "/AvatarPath";
        //    string absolutePath = "/Content/Upload/" + userId + "/AvatarPath/" + uploadedFile.GetHashCode() + ".jpg";
        //    string fullFilePath = rootPath + absolutePath;

        //    if (!Directory.Exists(directoryPath))
        //    {
        //        Directory.CreateDirectory(directoryPath);
        //    }

        //    if (!File.Exists(fullFilePath))
        //    {
        //        _fileStorage.SaveImageFileAsync(uploadedFile, fullFilePath);
        //    }

        //    return absolutePath;
        //}


        public void SaveFile(IFormFile uploadedFile, string fullPath)
        {
            string directoryPath = Path.GetDirectoryName(fullPath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(fullPath))
            {
                _fileStorage.SaveImageFileAsync(uploadedFile,fullPath);
            }
        }


        public async Task<string> GetTextContentFromFile(string localPath)
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


        public void CopyFileToDirectory(string sourceFileName, string destFileName)
        {
            if (File.Exists(sourceFileName))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(destFileName));
                File.Copy(sourceFileName, destFileName, true);
            }

        }


        public void WriteToExcel(DataTable table, string fullPath)
        {
            FileInfo file = new FileInfo(fullPath);
            DeleteFileIfExist(fullPath);
            _reportService.WriteToExcel(table, file); //EPPlus or OpenXml realization
        }

        
        public async void SaveFromExcelToDb(string fullPath, int gymId)   //VisitingChartModel only
        {
            List<VisitingChartModel> output = await ReadFromExcel(fullPath);
            foreach (var model in output)
            {
                model.GymId = gymId;
            }

            _fileStorage.AddVisitingChartDataForGym(output);
        }


        private async Task<List<VisitingChartModel>> ReadFromExcel(string fullPath)  //VisitingChartModel only
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
