using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Gyms")]
    class GymEntity
    {
        public GymEntity()
        {
            TrainerGymRelation = new HashSet<TrainerGymEntity>();
        }
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public ICollection<TrainerGymEntity> TrainerGymRelation { get; set; }
    }
}
