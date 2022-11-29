using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class EventEntityBase
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
        public string TrainerId { get; set; }
        public string UserId { get; set; }
        public int TrainingId { get; set; }
        public string Status { get; set; }



    }
}
