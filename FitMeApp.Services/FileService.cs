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
    public class FileService: IFileService
    {
        private readonly IFileStorage _fileStorage;
        private readonly IReportService _reportService;

        public FileService(IFileStorage fileStorage, IReportService reportServiceService)
        {
            _fileStorage = fileStorage;
            _reportService = reportServiceService;
        }


        public string SaveAvatarFileAsync(string userId,  IFormFile uploadedFile, string rootPath)
        {
            string directoryPath = rootPath + "/Content/Upload/" + userId + "/AvatarPath";
            string absolutePath = "/Content/Upload/" + userId + "/AvatarPath/" + uploadedFile.GetHashCode() + ".jpg";
            string fullFilePath = rootPath + absolutePath;

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(fullFilePath))
            {
               _fileStorage.SaveImageFileAsync(uploadedFile,fullFilePath);
            }

            return absolutePath;
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


        public void WriteToExcel(DataTable table, string fullPath) 
        {
            FileInfo file = new FileInfo(fullPath);
            DeleteFileIfExist(file);
            _reportService.WriteToExcel(table, file); //EPPlus or OpenXml realization
        }

        //todo add method "SaveInputExcelFile(int gymId, IFormFile? file,  string fullPath)"
        //todo ReadFromExcel -> private (return output)
        //todo AddToDb private method

        public async Task<List<VisitingChartModel>> ReadFromExcel(string fullPath) //todo thinking about method Name 
        {
            FileInfo file = new FileInfo(fullPath);
            List<VisitingChartModel> output = await _reportService.ReadFromExcel(file);
            _fileStorage.AddVisitingChartDataForGym(output);
            return output;
        }




        private void DeleteFileIfExist(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
         

        public string SetUniqueFileName()
        {
            var fileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            fileName = Regex.Replace(fileName, "[^a-zA-Z0-9]", "");
            return fileName;
        }

    }
}
