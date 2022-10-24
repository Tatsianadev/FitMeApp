﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Trainers")]
    public class TrainerEntity
    {
        public TrainerEntity()
        {
            Gyms = new HashSet<GymEntity>();
            GroupClasses = new HashSet<GroupClassEntity>();
        }

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public ICollection<GymEntity> Gyms { get; set; }
        public ICollection<GroupClassEntity> GroupClasses { get; set; }
    }
}