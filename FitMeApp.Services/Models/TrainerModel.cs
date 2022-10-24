using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Models
{
    public class TrainerModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public ICollection<GymModel> Gym { get; set; }
        public ICollection<GroupClassModel> GroupClass { get; set; }
    }
}
