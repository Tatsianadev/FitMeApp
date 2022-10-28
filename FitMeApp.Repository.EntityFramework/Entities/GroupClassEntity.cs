using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GroupClasses")]
    public class GroupClassEntity: GroupClassEntityBase
    {
        public GroupClassEntity()
        {
            this.Gyms = new HashSet<GymEntity>();
            this.Trainers = new HashSet<TrainerEntity>();
        }
        //[Key]
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }       

        public ICollection<GymEntity> Gyms { get; set; }
        public ICollection<TrainerEntity> Trainers { get; set; }

    }
}
