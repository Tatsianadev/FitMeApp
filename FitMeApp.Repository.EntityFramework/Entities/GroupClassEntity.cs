using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GroupClasses")]
    public class GroupClassEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TrainerId { get; set; }
        public int GymId { get; set; }
        public virtual TrainerEntity Trainer { get; set; }
        public virtual GymEntity Gym { get; set; }
    }
}
