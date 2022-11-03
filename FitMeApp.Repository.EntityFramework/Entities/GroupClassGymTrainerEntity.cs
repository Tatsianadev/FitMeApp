using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("GroupClassGym")]
    public class GroupClassGymTrainerEntity
    {
        [Key]
        public int Id { get; set; }
        public int GroupClassId { get; set; }
        public int GymId { get; set; }
        public int TrainerId { get; set; }
        public virtual GroupClassEntity GroupClass { get; set; }
        public virtual GymEntity Gym { get; set; }
        public virtual TrainerEntity Trainer { get; set; }
    }
}
