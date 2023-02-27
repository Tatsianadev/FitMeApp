﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.Services.Contracts.Interfaces
{
     public interface IFileService
     {
         string SaveAvatarFileAsync(string userId, IFormFile uploadedFile, string rootPath);
     }
}