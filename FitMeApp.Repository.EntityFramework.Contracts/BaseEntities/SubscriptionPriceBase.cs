using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class SubscriptionPriceBase
    {
        public int Id { get; set; }
        public int ValidDays { get; set; }
        public bool GroupTrainingInclude { get; set; }
        public bool DietMonitoring { get; set; }
        public int Price { get; set; }
    }
}
