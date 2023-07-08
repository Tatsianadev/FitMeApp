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
        public string ShortDescription { get; set; }
        public List<string> DetailedDescription { get; set; }
        public IEnumerable<TrainerViewModel> Trainers { get; set; }
    }
}
