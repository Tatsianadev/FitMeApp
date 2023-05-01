using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Models
{
    public class TrainerRoleAppFormViewModel
    {
        public string UserId { get; set; }
        [Required]
        public string ContractNumber { get; set; }
        [Required]
        public int GymId { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }
}
