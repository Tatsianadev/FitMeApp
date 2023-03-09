using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class TrainerEntityBase
    {
        [Key]
        [Required]
        public string Id { get; set; }      
        [Required]
        public string Specialization { get; set; }
        //public int GymId { get; set; }
        public int WorkLicenseId { get; set; }

    }
}
