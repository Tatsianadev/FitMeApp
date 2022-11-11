using FitMeApp.Repository.EntityFramework.Contracts.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace FitMeApp.Repository.EntityFramework.Entities
{
    [Table ("GymSubscriptions")]
    public class GymSubscriptionEntity: GymSubscriptionEntityBase
    {
        public GymEntity Gym { get; set; }
        public SubscriptionEntity Subscription { get; set; }
    }
}
