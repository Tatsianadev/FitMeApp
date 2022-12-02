using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    public class TrainerWorkHoursEntity
    {
        [Key]
        public int Id { get; set; }
        public string TrainerId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public int GemWorkHoursId { get; set; }
    }
}
