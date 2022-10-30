using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class GroupClassViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TrainerViewModel> Trainers { get; set; }
        public ICollection<GymViewModel> Gyms { get; set; }
    }
}
