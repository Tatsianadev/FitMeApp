using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Contracts.BaseEntities
{
    public class TrainerEntityBase
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Picture { get; set; }
        public string Specialization { get; set; }
    }
}
