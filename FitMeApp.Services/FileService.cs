using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FitMeApp.Common;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace FitMeApp.Services
{
    public class FileService: IFileService
    {
        private readonly IRepository _repository;
        private readonly IFileStorage _fileStorage;

        public FileService(IRepository repository, IFileStorage fileStorage)
        {
            _repository = repository;
            _fileStorage = fileStorage;
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


        public void WriteToExcel(DataTable table, string fullPath, string tableName) 
        {
            FileInfo file = new FileInfo(fullPath);
            DeleteFileIfExist(file);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var excelPack = new ExcelPackage(file))
            {
                var sheet = excelPack.Workbook.Worksheets.Add(tableName);
                sheet.Cells["A2"].LoadFromDataTable(table, true, OfficeOpenXml.Table.TableStyles.Light11);
                sheet.Cells.AutoFitColumns();
                
                //Formats the header
                sheet.Cells["A1"].Value = $"List of {tableName} of " + DateTime.Now.ToString("dd-MM-yyyy");
                sheet.Cells["A1:F1"].Merge = true;
                sheet.Rows[1].Style.Font.Size = 20 ;
                sheet.Rows[2].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                excelPack.Save();
            }
        }


        private void DeleteFileIfExist(FileInfo file)
        {
            if (file.Exists)
            {
                file.Delete();
            }
        }
         

        public string GetUniqueFileName()
        {
            var fileName = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            fileName = Regex.Replace(fileName, "[^a-zA-Z0-9]", "");
            return fileName;
        }

    }
}
