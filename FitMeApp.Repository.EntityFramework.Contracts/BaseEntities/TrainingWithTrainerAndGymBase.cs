using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class TrainingWithTrainerAndGymBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<GymEntityBase> Gyms { get; set; }
        public IEnumerable<TrainerEntityBase> Trainers { get; set; }
    }
}
