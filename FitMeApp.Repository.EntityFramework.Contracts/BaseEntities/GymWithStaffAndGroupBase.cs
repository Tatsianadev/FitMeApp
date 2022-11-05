﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class GymWithStaffAndGroupBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IEnumerable<TrainerEntityBase> Trainers { get; set; }
        public IEnumerable<GroupClassEntityBase> GroupClasses { get; set; }


    }
}
