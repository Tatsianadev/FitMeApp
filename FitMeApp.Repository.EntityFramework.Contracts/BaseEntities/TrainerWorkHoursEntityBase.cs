using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class TrainerWorkHoursEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int GymWorkHoursId { get; set; }
    }
}
