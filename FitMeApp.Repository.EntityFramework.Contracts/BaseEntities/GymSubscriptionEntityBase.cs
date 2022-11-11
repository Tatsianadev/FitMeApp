using System;
using System.Collections.Generic;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Contracts.BaseEntities
{
    public class GymSubscriptionEntityBase
    {
        public int Id { get; set; }
        public int GymId { get; set; }
        public int SubscriptionId { get; set; }
        public int Price { get; set; }
    }
}
