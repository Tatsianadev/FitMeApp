using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GroupClasses")]
    class GroupClassEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TrainerId { get; set; }
        public int GymId { get; set; }
        public TrainerEntity Trainer { get; set; }
        public GymEntity Gym { get; set; }
    }
}
