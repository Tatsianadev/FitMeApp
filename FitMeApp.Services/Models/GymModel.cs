using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Models
{
    public class GymModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ICollection<TrainerModel> TrainerStaff { get; set; }
        public ICollection<GroupClassModel> GroupClasses { get; set; }
    }
}
