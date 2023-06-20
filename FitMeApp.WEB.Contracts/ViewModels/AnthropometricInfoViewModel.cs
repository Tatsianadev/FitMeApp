using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class AnthropometricInfoViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        [Range(40, 220)]
        public int Height { get; set; }
        [Required]
        public int Weight { get; set; }
        [Required]
        [Range(1, 100)]
        public int Age { get; set; }
        [Required]
        public int PhysicalActivity { get; set; }
        public int CurrentCalorieIntake { get; set; }
        [Required]
        public string Goal { get; set; }
        public DateTime Date { get; set; }
        
    }
}
