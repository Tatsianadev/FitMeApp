﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class ApplyingForPersonalTrainingViewModel
    {
        [Required]
        public string TrainerId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }
        [Required]
        public int GymId { get; set; }
        public string GymName { get; set; }
        public string GymAddress { get; set; }
        public List<string> AvailableTime { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public DateTime SelectedDate { get; set; }
        [Required(ErrorMessage = "Please, choose start time")]
        public string SelectedStartTime { get; set; }
        [Required]
        public int DurationInMinutes { get; set; }
        public int Price { get; set; }
    }
}
