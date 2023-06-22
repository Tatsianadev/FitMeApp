using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FitMeApp.Common;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class AnthropometricInfoViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        [Range(40, 220)]
        public int Height { get; set; }

        [Required]
        public int Weight { get; set; }

        [Required]
        [Range(1, 100)]
        public int Age { get; set; }

        [Required]
        [DisplayName("Daily physical activity")]
        public int PhysicalActivity { get; set; }

        [DisplayName("Current calorie intake")]
        [Range(1, 10000)]
        public int CurrentCalorieIntake { get; set; }

        [Required]
        public DietGoalsEnum Goal { get; set; }
        public DateTime Date { get; set; }
        
    }
}
