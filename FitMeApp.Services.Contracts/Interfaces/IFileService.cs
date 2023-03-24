using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Common;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.Services.Contracts.Interfaces
{
     public interface IFileService
     {
         string SaveAvatarFileAsync(string userId, IFormFile uploadedFile, string rootPath);
         Task<string> GetTextContentFromFile(string localPath);
         void WriteToExcel(DataTable table, string fullPath, string tableName);
         string SetUniqueFileName();
     }
}
