using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class GymWorkHoursEntityBase
    {
        [Key]
        public int Id { get; set; }
        public DayOfWeek DayOfWeekNumber { get; set; }
        public int GymId { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }
}
