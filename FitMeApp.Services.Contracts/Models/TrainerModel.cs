﻿using FitMeApp.Common;
using System.Collections.Generic;


namespace FitMeApp.Services.Contracts.Models
{
    public class TrainerModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string AvatarPath { get; set; }
        public string Specialization { get; set; }
        public int WorkLicenseId { get; set; }
        public GymModel Gym { get; set; }       
        public ICollection<TrainingModel> Trainings { get; set; }
    }
}
