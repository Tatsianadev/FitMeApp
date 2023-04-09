using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class NumberOfVisitorsPerHourEntityBase
    {
        [Key]
        public int Id { get; set; }
        public int GymId { get; set; }
        public int DayOfWeekNumber { get; set; }
        public int Hour { get; set; }
        public int NumberOfVisitors { get; set; }
    }
}
