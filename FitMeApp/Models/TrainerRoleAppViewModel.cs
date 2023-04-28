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
        //public bool HasTrainerSubscription { get; set; }
        //public bool HasContract { get; set; }
        public string ContractNumber { get; set; }
        public int GymId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
