using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class GymWithTrainersAndGroupBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IEnumerable<TrainerWithGymAndGroupBase> Trainers { get; set; }
        //public IEnumerable<ClassEntityBase> GroupClasses { get; set; }


    }
}
