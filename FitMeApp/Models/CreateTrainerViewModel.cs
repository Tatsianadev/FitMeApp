using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Models
{
    public class CreateTrainerViewModel
    {
        //public string Login { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Year { get; set; }
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]       
        public string Picture { get; set; }
        [Required]
        public string Specialization { get; set; }        
        public int GymId { get; set; }
        public List<int> TrainingsId { get; set; }
    }
}
