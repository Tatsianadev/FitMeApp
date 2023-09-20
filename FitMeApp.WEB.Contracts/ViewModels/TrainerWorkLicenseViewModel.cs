using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FitMeApp.WEB.Contracts.Attributes;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    [StartDateEarlierThanEnd]
    public class TrainerWorkLicenseViewModel
    {
        public int Id { get; set; }
        public string TrainerId { get; set; }
        [Required(ErrorMessage = "Write the contract number over")]
        public string ContractNumber { get; set; }
        public int GymId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
