﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class GymViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ICollection<TrainerViewModel> TrainerStaff { get; set; }
        public ICollection<GroupClassViewModel> GroupClasses { get; set; }
    }
}
