using FitMeApp.Contracts.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("Gyms")]
    public class GymEntity:GymEntityBase
    {
        public GymEntity()
        {           
            Trainers = new HashSet<TrainerEntity>();
        }

        //[Key]
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Address { get; set; }
        //public string Phone { get; set; }
        public ICollection<TrainerEntity> Trainers { get; set; }
    }
}
