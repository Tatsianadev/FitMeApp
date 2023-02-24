﻿using FitMeApp.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class TrainerViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Gender { get; set; }
        public string Avatar { get; set; }
        public byte[] Image { get; set; }
        [Required]
        public string Specialization { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string Phone { get; set; }
        public int Year { get; set; }
        public TrainerApproveStatusEnum Status { get; set; }
        public GymViewModel Gym { get; set; }       
        public ICollection<TrainingViewModel> Trainings { get; set; }
       
    }
}
