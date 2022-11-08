using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table("ClassTrainer")]
    public class ClassTrainerEntity
    {
        [Key]
        public int Id { get; set; }
        public int ClassId { get; set; }        
        public int TrainerId { get; set; }
        public virtual ClassEntity Class { get; set; }       
        public virtual TrainerEntity Trainer { get; set; }
    }
}
