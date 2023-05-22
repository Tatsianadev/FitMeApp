using System;
using System.Collections.Generic;
using System.Text;
using FitMeApp.Common;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class TrainingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<TrainerViewModel> Trainers { get; set; }
    }
}
