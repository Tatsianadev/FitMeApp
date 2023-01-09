using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Models
{
    public class TrainerRoleAppViewModel
    {
        public string Id { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public int Year { get; set; }
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public string Specialization { get; set; }
        [Required]
        public int GymId { get; set; }
        public string GymName { get; set; }
        [Required]
        public List<int> TrainingsId { get; set; }
        public TrainerApproveStatusEnum Status { get; set; }
    }
}
