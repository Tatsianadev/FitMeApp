using FitMeApp.Common;
using System.ComponentModel.DataAnnotations;

namespace FitMeApp.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[Display(Name = "Phone number")]
        //public string PhoneNumber { get; set; }

        //[Display(Name = "Year of birth")]
        //public int Year { get; set; }

        //[Display(Name = "Gender")]
        //public string Gender { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Passwords mismatch")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        public string PasswordConfirm { get; set; }

        public RolesEnum Role { get; set; }
    }
}
