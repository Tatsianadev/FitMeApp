using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Trainers")]
    class TrainerEntity
    {
        public TrainerEntity()
        {
            Gym = new HashSet<GymEntity>();
            GroupClass = new HashSet<GroupClassEntity>();
        }

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public ICollection<GymEntity> Gym { get; set; }
        public ICollection<GroupClassEntity> GroupClass { get; set; }
    }
}
