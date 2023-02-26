using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FitMeApp.Services.Contracts.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.Services
{
    public class FileStorage: IFileStorage
    {

        public async void SaveImageFileAsync(IFormFile uploadedFile, string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
            }
        }
    }
}
