﻿using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Models
{
    public class GymModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IEnumerable<string> GymImagePaths { get; set; }
        public ICollection<TrainerModel> Trainers { get; set; }
        //public ICollection<GroupClassModel> GroupClasses { get; set; }
    }
}
