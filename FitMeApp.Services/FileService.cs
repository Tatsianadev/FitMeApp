using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using FitMeApp.Repository.EntityFramework.Contracts.Interfaces;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;

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
    }
}
