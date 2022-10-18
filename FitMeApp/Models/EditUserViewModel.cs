using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FitMeApp.Models
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
       
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int Year { get; set; }
        public string Gender { get; set; }
    }
}
