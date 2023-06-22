using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Services.Contracts.Interfaces
{
    public interface IDietService
    {
        int AddAnthropometricInfo(AnthropometricInfoModel infoModel);
    }
}
