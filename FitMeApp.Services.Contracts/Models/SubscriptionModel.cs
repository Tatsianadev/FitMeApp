﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Services.Contracts.Models
{
    public class SubscriptionModel
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public int ValidDays { get; set; }
        public bool GroupTraining { get; set; }
        public bool DietMonitoring { get; set; }
        public bool WorkAsTrainer { get; set; }
        public int Price { get; set; }
    }
}
