using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.WEB.Contracts.ViewModels
{
    public class SubscriptionViewModel
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public int ValidDays { get; set; }
        public bool GroupTraining { get; set; }
        public bool DietMonitoring { get; set; }
        public int Price { get; set; }
        public string Image { get; set; }
    }
}
