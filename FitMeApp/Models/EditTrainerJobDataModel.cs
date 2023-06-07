using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FitMeApp.Services.Contracts.Models;

namespace FitMeApp.Models
{
    public class EditTrainerJobDataModel
    {
        public string Id { get; set; }
        [Required]
        public string Specialization { get; set; }
        [Required]
        public int GymId { get; set; }
        public string GymName { get; set; }
        [Required]
        public List<int> TrainingsId { get; set; }
        [Range(0,200, ErrorMessage = "Price should be in range from 0 to 200")]
        public int PricePerHour { get; set; }
        public TrainerWorkLicenseModel WorkLicense { get; set; }
    }
}
