using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Contracts.BaseEntities
{
    public class GroupClassEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TrainerId { get; set; }
        public int GymId { get; set; }
    }
}
