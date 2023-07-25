using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Services.Contracts.Models.JsonModels;

namespace FitMeApp.Services.Contracts.Models
{
    public class HomeTrainingModel: HomeTrainingJsonModel
    {
        public string ShortDescription { get; set; }
    }
}
