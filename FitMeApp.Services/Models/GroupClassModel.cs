using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Models
{
    public class GroupClassModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<TrainerModel> Trainers { get; set; }
        public ICollection<GymModel> Gyms { get; set; }

    }
}
