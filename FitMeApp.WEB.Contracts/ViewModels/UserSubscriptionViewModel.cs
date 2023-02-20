﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class UserSubscriptionViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int GymSubscriptionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool GroupTraining { get; set; }
        public bool DietMonitoring { get; set; }
    }
}
