﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.JoinEntitiesBase
{
    public class UserSubscriptionFullInfoBase
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public int GymId { get; set; }
        public string GymName { get; set; }
        public string TrainerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool GroupTraining { get; set; }
        public bool DietMonitoring { get; set; }
        public bool WorkAsTrainer { get; set; }
        public int Price { get; set; }
    }
}
