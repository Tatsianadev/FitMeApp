using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class TrainerViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public string Specialization { get; set; }
        public GymViewModel Gym { get; set; }
        public ICollection<GroupClassViewModel> GroupClasses { get; set; }
    }
}
