using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class SubscriptionTypeEntityBase
    {
        public int Id { get; set; }
        public string LifePeriod { get; set; }
        public bool GroupClassInclude { get; set; }
        public bool DietMonitoring { get; set; }
        public int GymId { get; set; }
        public int Price { get; set; }
        
    }
}
